let pointsBuffer = [];
let initialLoad = true;

// Function to initialize the chart
function initializeChart() {
    window.chart = new ApexCharts(document.querySelector("#temperatureChart"), {
        chart: {
            id: 'realtime',
            type: 'line',
            height: 350,
            animations: {
                enabled: true,
                easing: 'linear',
                dynamicAnimation: {
                    speed: 1000 // Smooth animations over 1 second
                }
            },
            toolbar: {
                show: true,
                tools: {
                    download: true,
                    selection: true,
                    zoom: true,
                    zoomin: true,
                    zoomout: true,
                    pan: true,
                    reset: true
                },
                autoSelected: 'zoom'
            },
            zoom: {
                enabled: true,
                type: 'x',
                autoScaleYaxis: true
            }
        },
        series: [{
            name: 'Temperature',
            data: pointsBuffer // Initial empty dataset
        }],
        xaxis: {
            type: 'datetime',
            range: 120000 // Display data for the last 120000 milliseconds
        },
        yaxis: {
            min: 20, 
            max: 80, 
            title: {
                text: 'Temperature (°C)'
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth'
        },
        markers: {
            size: 0
        },
        legend: {
            show: false
        },
        tooltip: {
            x: {
                format: 'HH:mm:ss'
            }
        },
    });

    window.chart.render();
}

// Function to add new data to the points buffer and update the chart data array
function addData(newData) {
    const existingTimestamps = pointsBuffer.map(item => item[0]);

    newData.forEach(item => {
        const timestamp = new Date(item.timestamp).getTime();
        const isClose = existingTimestamps.some(existingTimestamp => {
            return Math.abs(existingTimestamp - timestamp) <= 100; // Check if within 100ms
        });

        if (!isClose) {
            const newDataPoint = [timestamp, item.temperature];
            pointsBuffer.push(newDataPoint);
        }
    });

    // Ensure pointsBuffer does not grow indefinitely
    const maxDuration = 120000; // 2 minutes
    const currentTime = new Date().getTime();
    pointsBuffer = pointsBuffer.filter(point => {
        return currentTime - point[0] <= maxDuration;
    });

    // Update the chart with the new pointsBuffer
    window.chart.updateSeries([{
        data: pointsBuffer
    }]);
}

// Function to fetch temperature data using AJAX
function fetchTemperatureData(pointsCount) {
    $.ajax({
        url: `/temp?n=${pointsCount}`,
        method: 'GET',
        success: (data) => {
            addData(data);
        },
        error: (error) => {
            console.error('Error fetching temperature data:', error);
        }
    });
}

// Initial chart load
initializeChart();
fetchTemperatureData(200); // Fetch 200 points on initial load

// Set interval to fetch new data every 1000 milliseconds
setInterval(() => {
    if (initialLoad) {
        initialLoad = false;
    } else {
        fetchTemperatureData(5); // Fetch 5 points on subsequent loads
    }
}, 1000);
