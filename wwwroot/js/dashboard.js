// Global array to store the latest data points fetched from the API
let apiData = [];

// Function to initialize the chart
function initializeChart() {
    const ctx = document.getElementById('temperatureChart');
    window.chart = new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Temperature',
                data: [], // Initial empty dataset
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 3,
                fill: false,
                cubicInterpolationMode: 'monotone',
                pointRadius: 0 // Make individual dots invisible
            }]
        },
        options: {
            animation: false,
            scales: {
                x: {
                    type: 'realtime',
                    realtime: {
                        duration: 20000, // Display data for the last 20000 milliseconds
                        refresh: 1000,   // Refresh interval in milliseconds
                        delay: 3000,     // Increase delay to 2000 milliseconds (2 seconds)
                        onRefresh: (chart) => {
                            // No additional processing in onRefresh
                            // The data will already be interpolated and added in `fetchTemperatureData`
                        }
                    },
                    title: {
                        display: true,
                        text: 'Time'
                    }
                },
                y: {
                    min: 20, // Minimum y value for the temperature
                    max: 80, // Maximum y value for the temperature
                    title: {
                        display: true,
                        text: 'Temperature (°C)'
                    }
                }
            }
        }
    });
}

// Function to add new data to the global array and update the chart
function addData(newData) {
    const existingTimestamps = new Set(window.chart.data.datasets[0].data.map(item => item.x.getTime()));
    var lastPoint = window.chart.data.datasets[0].data.length > 0 
                      ? window.chart.data.datasets[0].data[window.chart.data.datasets[0].data.length - 1] 
                      : null;

    // Process and interpolate new data points
    const interpolatedPoints = [];
    newData.forEach(item => {
        const timestamp = new Date(item.timestamp).getTime();
        if (!existingTimestamps.has(timestamp)) {
            const newDataPoint = {
                x: new Date(item.timestamp),
                y: item.temperature
            };
            console.log("New point: ", newDataPoint)
            interpolatedPoints.push(...interpolatePoints(lastPoint, [newDataPoint]));
            lastPoint = newDataPoint;
            existingTimestamps.add(timestamp);
        }
    });

    // Add the interpolated points directly to the chart data
    window.chart.data.datasets[0].data.push(...interpolatedPoints);
    window.chart.update();

}

// Function to interpolate data points for smooth transition
function interpolatePoints(lastPoint, newData) {
    const interpolationQuantity = 5
    const interpolatedPoints = [];
    if (lastPoint) {
        // Interpolate between the last point from existing data and the first new data point
        const firstNew = newData[0];
        const delta = (firstNew.x - lastPoint.x) / interpolationQuantity;

        for (let i = 1; i < interpolationQuantity; i++) {
            interpolatedPoints.push({
                x: new Date(lastPoint.x.getTime() + i * delta),
                y: lastPoint.y + i * (firstNew.y - lastPoint.y) / interpolationQuantity
            });
        }
    }

    // Interpolate between successive new data points
    for (let i = 0; i < newData.length - 1; i++) {
        const startPoint = newData[i];
        const endPoint = newData[i + 1];
        const delta = (endPoint.x - startPoint.x) / interpolationQuantity;

        for (let j = 1; j < interpolationQuantity; j++) {
            interpolatedPoints.push({
                x: new Date(startPoint.x.getTime() + j * delta),
                y: startPoint.y + j * (endPoint.y - startPoint.y) / interpolationQuantity
            });
        }
    }

    // Add the new data points themselves at the end
    interpolatedPoints.push(...newData);
    
    return interpolatedPoints;
}

// Function to fetch temperature data using AJAX
function fetchTemperatureData() {
    $.ajax({
        url: '/temp',
        method: 'GET',
        success: (data) => {
            addData(data); // Directly call addData to process and interpolate
        },
        error: (error) => {
            console.error('Error fetching temperature data:', error);
        }
    });
}

// Initial chart load
initializeChart();
fetchTemperatureData();

// Set interval to fetch new data every 1000 milliseconds
setInterval(fetchTemperatureData, 1000);
