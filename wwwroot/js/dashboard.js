function renderChart(data) {
    var ctx = document.getElementById('temperatureChart').getContext('2d');

    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.Timestamp),
            datasets: [{
                label: 'Temperature',
                data: data.map(item => item.Temperature),
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1,
                fill: false
            }]
        },
        options: {
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'hour',
                        tooltipFormat: 'HH:mm',
                        displayFormats: {
                            hour: 'HH:mm'
                        }
                    },
                    title: {
                        display: true,
                        text: 'Time'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Temperature (°C)'
                    }
                }
            }
        }
    });
}