var ctxPie = document.getElementById('pieChart').getContext('2d');
var pieChart = new Chart(ctxPie, {
    type: 'pie',
    data: {
        labels: ['Active Users', 'Non-Active Users'],
        datasets: [{
            label: '# of Users',
            data: [activeUsersCount, nonActiveUsersCount],
            backgroundColor: [
                'rgb(75, 192, 192)',
                'rgb(255, 99, 132)'
            ],
            hoverOffset: 4
        }]
    }
});

// Random data generation function






