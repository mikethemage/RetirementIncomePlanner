jQuery(document).ready(function($) {
    $('#retirement-income-planner-form').submit(function(e) {
      e.preventDefault(); // Prevent form submission
  
      // Get form field values
      var numberOfYears = $('#numberOfYears').val();
      var indexation = $('#indexation').val();
      var retirementPot = $('#retirementPot').val();
      var investmentGrowth = $('#investmentGrowth').val();
  
      var client1 = {
        age: $('#client1Age').val(),
        salaryDetails: {
          fullSalaryAmount: $('#client1FullSalaryAmount').val(),
          partialRetirementDetails: {
            age: $('#client1PartialRetirementAge').val(),
            amount: $('#client1PartialRetirementAmount').val()
          }
        },
        retirementAge: $('#client1RetirementAge').val(),
        statePensionAmount: $('#client1StatePensionAmount').val(),
        statePensionAge: $('#client1StatePensionAge').val(),
        otherPensionDetails: {
          age: $('#client1OtherPensionAge').val(),
          amount: $('#client1OtherPensionAmount').val()
        },
        otherIncome: $('#client1OtherIncome').val(),
        retirementIncomeLevel: $('#client1RetirementIncomeLevel').val(),
        adhocTransactions: [
          {
            age: $('#client1AdhocTransactionAge').val(),
            amount: $('#client1AdhocTransactionAmount').val()
          }
        ]
      };
  
      var client2 = {
        age: $('#client2Age').val(),
        salaryDetails: {
          fullSalaryAmount: $('#client2FullSalaryAmount').val(),
          partialRetirementDetails: {
            age: $('#client2PartialRetirementAge').val(),
            amount: $('#client2PartialRetirementAmount').val()
          }
        },
        retirementAge: $('#client2RetirementAge').val(),
        statePensionAmount: $('#client2StatePensionAmount').val(),
        statePensionAge: $('#client2StatePensionAge').val(),
        otherPensionDetails: {
          age: $('#client2OtherPensionAge').val(),
          amount: $('#client2OtherPensionAmount').val()
        },
        otherIncome: $('#client2OtherIncome').val(),
        retirementIncomeLevel: $('#client2RetirementIncomeLevel').val(),
        adhocTransactions: [
          {
            age: $('#client2AdhocTransactionAge').val(),
            amount: $('#client2AdhocTransactionAmount').val()
          }
        ]
      };
  
      // Prepare the data for the API request
      var data = {
        numberOfYears: numberOfYears,
        indexation: indexation,
        retirementPot: retirementPot,
        investmentGrowth: investmentGrowth,
        clients: [client1, client2]
      };
  
      // Make the AJAX request to the proxy PHP script
      $.ajax({
        url: retirementIncomePlannerAjax.ajaxurl, // Use the localized AJAX URL
        type: 'POST',
        dataType: 'json',
        data: {
          action: 'retirement_income_planner_proxy_request',
          data: JSON.stringify(data)
        },
        success: function(response) {
          console.log(response); // For debugging purposes
          var responseData = JSON.parse(response);

          // Create the table header
        var table = '<table>';
        table += '<tr><th>Year</th>';

        table += '<th>Client 1 Age</th>' 
        table += '<th>Client 1 State Pension</th>' 
        table += '<th>Client 1 Other Pension</th>' 
        table += '<th>Client 1 Salary</th>' 
        table += '<th>Client 1 Other Income</th>'
        table += '<th>Client 1 Contribution</th>'

        table += '<th>Client 2 Age</th>' 
        table += '<th>Client 2 State Pension</th>' 
        table += '<th>Client 2 Other Pension</th>' 
        table += '<th>Client 2 Salary</th>' 
        table += '<th>Client 2 Other Income</th>'
        table += '<th>Client 2 Contribution</th>'

        table += '</tr>';

        // Iterate over the response data and create table rows
        for (var i = 0; i < responseData.length; i++) {
          var row = responseData[i];

          // Create table row
          table += '<tr>';
          table += '<td>' + row.year + '</td>';

          // Find client 1 data
          var client1 = row.clients.find(function(client) {
            return client.clientNumber === 1;
          });

          if (client1) {
            table +=  getClientDataHtml(client1);
          } else {
            table += '<td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td>';
          }

          // Find client 2 data
          var client2 = row.clients.find(function(client) {
            return client.clientNumber === 2;
          });

          if (client2) {
            table += getClientDataHtml(client2);
          } else {
            table += '<td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td>';
          }

          table += '</tr>';
        }

        // Close the table
        table += '</table>';

        // Display the table
        $('#retirement-income-planner-result').html(table);
          },
        error: function(xhr, status, error) {
          console.log(xhr.responseText); // For debugging purposes
          $('#retirement-income-planner-result').text('Error: ' + error);
        }
      });
    });
  });
  
   // Function to format client data as HTML
   function getClientDataHtml(client) {
    var html = '';
    html += '<td>' + client.age + '</td>';
    html += '<td>' + client.statePension + '</td>';
    html += '<td>' + client.otherPension + '</td>';
    html += '<td>' + client.salary + '</td>';
    html += '<td>' + client.otherIncome + '</td>';
    html += '<td>' + client.contribution+ '</td>';
    return html;
  }