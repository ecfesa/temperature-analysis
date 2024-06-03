// Register data labels chartjs plugin and zoom plugin
Chart.register(ChartDataLabels);

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
                pointRadius: 0
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
                        delay: 3000,     // 3 seconds delay for smoother transitions
                        onRefresh: (chart) => {
                            // No additional processing in onRefresh
                            // The data will already be added in `fetchTemperatureData`
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
            },
            plugins: {
                datalabels: {
                    // Assume x axis has the realtime scale
                    backgroundColor: context => context.dataset.borderColor,
                    padding: 4,
                    borderRadius: 4,
                    clip: true,       // true is recommended to keep labels from running off the chart area
                    color: 'white',
                    font: {
                        weight: 'bold'
                    },
                    formatter: value => `${value.y.toFixed(1)}°C` // Use template literal
                },
                zoom: {
                    limits: {
                        x: {  
                            minDelay: 0,         // Set minimum delay limit
                            maxDelay: 20000,     // Set maximum delay limit to match duration
                            minDuration: 1000,   // Set minimum duration to 1 second
                            maxDuration: 60000   // Set maximum duration to 60 seconds
                        }
                    },
                    pan: {
                        enabled: true, // Enable panning
                        mode: 'x'      // Allow panning in the x direction
                    },
                    zoom: {
                        pinch: {
                            enabled: true // Enable pinch zooming
                        },
                        wheel: {
                            enabled: true // Enable wheel zooming
                        },
                        mode: 'x' // Allow zooming in the x direction
                    }
                }
            }
        }
    });
}

// Function to add new data to the global array and update the chart
function addData(newData) {
    const existingTimestamps = new Set(window.chart.data.datasets[0].data.map(item => item.x.getTime()));

    newData.forEach(item => {
        const timestamp = new Date(item.timestamp).getTime();
        if (!existingTimestamps.has(timestamp)) {
            const newDataPoint = {
                x: new Date(item.timestamp),
                y: item.temperature
            };
            window.chart.data.datasets[0].data.push(newDataPoint);
        }
    });

    window.chart.update();
}

// Function to fetch temperature data using AJAX
function fetchTemperatureData() {
    $.ajax({
        url: '/temp',
        method: 'GET',
        success: (data) => {
            addData(data); // Directly call addData to process and add
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
