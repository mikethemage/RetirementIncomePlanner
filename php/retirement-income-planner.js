//Code to hide/display Client 2 based on drop-down:
var clientToggle = document.getElementById("NumberOfClients");
var clientInfo = document.getElementById("client-info");

clientToggle.addEventListener("change", function () {
  var clientInputs2 = document.querySelector(".client-inputs:nth-child(2)");

  if (clientToggle.value == 2) {
    clientInputs2.style.display = "block";
  } else {
    clientInputs2.style.display = "none";
  }
});



jQuery(document).ready(function ($) {
  $('#retirement-income-planner-form').submit(function (e) {
    e.preventDefault(); // Prevent form submission

    // Get form field values
    var numberOfYears = $('#numberOfYears').val();
    var indexationPercentage = $('#indexation').val();
    var retirementPot = $('#retirementPot').val();
    var investmentGrowthPercentage = $('#investmentGrowth').val();

    // Convert percentage values to decimal format
    var indexation = parseFloat(indexationPercentage) / 100;
    var investmentGrowth = parseFloat(investmentGrowthPercentage) / 100;

    var client1 = {
      age: $('#client1Age').val(),
      retirementAge: $('#client1RetirementAge').val(),
      statePensionAmount: $('#client1StatePensionAmount').val(),
      statePensionAge: $('#client1StatePensionAge').val(),
      retirementIncomeLevel: $('#client1RetirementIncomeLevel').val()
    };

    // Check if fullSalaryAmount is filled
    var fullSalaryAmount = $('#client1FullSalaryAmount').val();
    if (fullSalaryAmount) {
      client1.salaryDetails = {
        fullSalaryAmount: fullSalaryAmount
      };

      // Check if partial retirement details are filled
      if ($('#client1PartialRetirementAge').val() && $('#client1PartialRetirementAmount').val()) {
        client1.salaryDetails.partialRetirementDetails = {
          age: $('#client1PartialRetirementAge').val(),
          amount: $('#client1PartialRetirementAmount').val()
        };
      }
    }

    // Check if otherPensionDetails are filled:
    if ($('#client1OtherPensionAge').val() && $('#client1OtherPensionAmount').val()) {
      client1.otherPensionDetails = {
        age: $('#client1OtherPensionAge').val(),
        amount: $('#client1OtherPensionAmount').val()
      };
    }

    // Check if otherIncome is filled:
    if ($('#client1OtherIncome').val()) {
      client1.otherIncome = $('#client1OtherIncome').val()
    }

    // Check if adhocTransactions are filled:
    if ($('#client1AdhocTransactionAge').val() && $('#client1AdhocTransactionAmount').val()) {
      client1.adhocTransactions = [
        {
          age: $('#client1AdhocTransactionAge').val(),
          amount: $('#client1AdhocTransactionAmount').val()
        }
      ];
    }


    if (clientToggle.value == 2) {
      var client2 = {
        age: $('#client2Age').val(),
        retirementAge: $('#client2RetirementAge').val(),
        statePensionAmount: $('#client2StatePensionAmount').val(),
        statePensionAge: $('#client2StatePensionAge').val(),
        retirementIncomeLevel: $('#client2RetirementIncomeLevel').val()
      };

      // Check if fullSalaryAmount is filled
      var fullSalaryAmount = $('#client2FullSalaryAmount').val();
      if (fullSalaryAmount) {
        client2.salaryDetails = {
          fullSalaryAmount: fullSalaryAmount
        };

        // Check if partial retirement details are filled
        if ($('#client2PartialRetirementAge').val() && $('#client2PartialRetirementAmount').val()) {
          client2.salaryDetails.partialRetirementDetails = {
            age: $('#client2PartialRetirementAge').val(),
            amount: $('#client2PartialRetirementAmount').val()
          };
        }
      }

      // Check if otherPensionDetails are filled:
      if ($('#client2OtherPensionAge').val() && $('#client2OtherPensionAmount').val()) {
        client2.otherPensionDetails = {
          age: $('#client2OtherPensionAge').val(),
          amount: $('#client2OtherPensionAmount').val()
        };
      }

      // Check if otherIncome is filled:
      if ($('#client2OtherIncome').val()) {
        client2.otherIncome = $('#client2OtherIncome').val()
      }

      // Check if adhocTransactions are filled:
      if ($('#client2AdhocTransactionAge').val() && $('#client2AdhocTransactionAmount').val()) {
        client2.adhocTransactions = [
          {
            age: $('#client2AdhocTransactionAge').val(),
            amount: $('#client2AdhocTransactionAmount').val()
          }
        ];
      }

      // Prepare the data for the API request
      var data = {
        numberOfYears: numberOfYears,
        indexation: indexation,
        retirementPot: retirementPot,
        investmentGrowth: investmentGrowth,
        clients: [client1, client2]
      };
    }
    else {
      // Prepare the data for the API request
      var data = {
        numberOfYears: numberOfYears,
        indexation: indexation,
        retirementPot: retirementPot,
        investmentGrowth: investmentGrowth,
        clients: [client1]
      };
    }

    // Make the AJAX request to the proxy PHP script
    $.ajax({
      url: retirementIncomePlannerAjax.ajaxurl, // Use the localized AJAX URL
      type: 'POST',
      dataType: 'json',
      data: {
        action: 'retirement_income_planner_proxy_request',
        data: JSON.stringify(data)
      },
      success: function (response) {
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

        if (clientToggle.value == 2) {
          table += '<th>Client 2 Age</th>'
          table += '<th>Client 2 State Pension</th>'
          table += '<th>Client 2 Other Pension</th>'
          table += '<th>Client 2 Salary</th>'
          table += '<th>Client 2 Other Income</th>'
          table += '<th>Client 2 Contribution</th>'
        }

        // Add table headers for additional fields
        table += '<th>Total Required Drawdown</th>';
        table += '<th>Fund Before Drawdown</th>';
        table += '<th>Total Drawdown</th>';
        table += '<th>Total Fund Value</th>';

        table += '</tr>';

        // Iterate over the response data and create table rows
        for (var i = 0; i < responseData.length; i++) {
          var row = responseData[i];

          // Create table row
          table += '<tr>';
          table += '<td>' + row.year + '</td>';

          // Find client 1 data
          var client1 = row.clients.find(function (client) {
            return client.clientNumber === 1;
          });

          if (client1) {
            table += getClientDataHtml(client1);
          } else {
            table += '<td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td>';
          }

          if (clientToggle.value == 2) {

            // Find client 2 data
            var client2 = row.clients.find(function (client) {
              return client.clientNumber === 2;
            });

            if (client2) {
              table += getClientDataHtml(client2);
            } else {
              table += '<td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td>';
            }
          }

          // Add additional table cells
          table += '<td>' + row.totalRequiredDrawdown.toFixed(2) + '</td>';
          table += '<td>' + row.fundBeforeDrawdown.toFixed(2) + '</td>';
          table += '<td>' + row.totalDrawdown.toFixed(2) + '</td>';
          table += '<td>' + row.totalFundValue.toFixed(2) + '</td>';

          table += '</tr>';
        }

        // Close the table
        table += '</table>';

        // Display the table
        $('#retirement-income-planner-result').html(table);
      },
      error: function (xhr, status, error) {
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
  html += '<td>' + client.statePension.toFixed(2) + '</td>';
  html += '<td>' + client.otherPension.toFixed(2) + '</td>';
  html += '<td>' + client.salary.toFixed(2) + '</td>';
  html += '<td>' + client.otherIncome.toFixed(2) + '</td>';
  html += '<td>' + client.contribution.toFixed(2) + '</td>';
  return html;
}

