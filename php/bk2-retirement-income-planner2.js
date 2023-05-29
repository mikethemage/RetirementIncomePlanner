jQuery(document).ready(function($) {
  $('#retirement-income-planner-form').submit(function(e) {
    e.preventDefault(); // Prevent form submission

    // Get form field values
    // ...

    // Make the AJAX request to the proxy PHP script
    $.ajax({
      // ...
      success: function(response) {
        console.log(response); // For debugging purposes
        var responseData = JSON.parse(response);

        // Create the table header
        var table = '<table>';
        table += '<tr><th>Year</th>';

        // Add table headers for client 1 properties
        for (var prop in responseData[0].clients[0]) {
          table += '<th>Client 1 ' + prop + '</th>';
        }

        // Add table headers for client 2 properties
        for (var prop in responseData[0].clients[1]) {
          table += '<th>Client 2 ' + prop + '</th>';
        }

        table += '</tr>';

        // Iterate over the response data and create table rows
        for (var i = 0; i < responseData.length; i++) {
          var row = responseData[i];

          // Create table row
          table += '<tr>';
          table += '<td>' + row.year + '</td>';

          // Add table cells for client 1
          for (var prop in row.clients[0]) {
            table += '<td>' + row.clients[0][prop] + '</td>';
          }

          // Add table cells for client 2
          for (var prop in row.clients[1]) {
            table += '<td>' + row.clients[1][prop] + '</td>';
          }

          table += '</tr>';
        }

        // Close the table
        table += '</table>';

        // Display the table
        $('#retirement-income-planner-result').html(table);
      },
      // ...
    });
  });
});