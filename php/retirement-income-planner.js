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
  });

  // Handle JSON data retrieval
  $('#json-button').on('click', function (event) {
    var api_url = external_api_params.api_url;
    // Access the security nonce value from the localized object
    var security = external_api_params.security_json;

    // Gather input values
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
      client1.otherIncome = $('#client1OtherIncome').val();
    }

    // Check if adhocTransactions are filled:
    if ($('#client1AdhocTransactionAge').val() && $('#client1AdhocTransactionAmount').val()) {
      client1.adhocTransactions = [{
        age: $('#client1AdhocTransactionAge').val(),
        amount: $('#client1AdhocTransactionAmount').val()
      }];
    }

    var clients = [client1];

    var clientToggle = document.getElementById('NumberOfClients');
    if (clientToggle.value == 2) {
      var client2 = {
        age: $('#client2Age').val(),
        retirementAge: $('#client2RetirementAge').val(),
        statePensionAmount: $('#client2StatePensionAmount').val(),
        statePensionAge: $('#client2StatePensionAge').val(),
        retirementIncomeLevel: $('#client2RetirementIncomeLevel').val()
      };

      // Check if fullSalaryAmount is filled
      var fullSalaryAmountClient2 = $('#client2FullSalaryAmount').val();
      if (fullSalaryAmountClient2) {
        client2.salaryDetails = {
          fullSalaryAmount: fullSalaryAmountClient2
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
        client2.otherIncome = $('#client2OtherIncome').val();
      }

      // Check if adhocTransactions are filled:
      if ($('#client2AdhocTransactionAge').val() && $('#client2AdhocTransactionAmount').val()) {
        client2.adhocTransactions = [{
          age: $('#client2AdhocTransactionAge').val(),
          amount: $('#client2AdhocTransactionAmount').val()
        }];
      }

      clients.push(client2);
    }


    // Prepare the data for the API request
    var data = {
      action: 'external_api_json_request',
      security: security,
      pensionInputData: {
        numberOfYears: numberOfYears,
        indexation: indexation,
        retirementPot: retirementPot,
        investmentGrowth: investmentGrowth,
        clients: clients
      }
    }

    $.post(api_url, data, function (response) {
      if (response.success) {

        var json_data = JSON.parse(response.data.json_data);

        var html_table = '<table><tbody>';
        // Generate HTML table from the JSON data
        // Modify this section according to your JSON structure
        html_table += '<tr><th>Year</th>';

        for (var j = 0; j < json_data[0].clients.length; j++)
          if (json_data[0].clients.length > 1) {
            html_table += '<th>Client ' + (j + 1) + ' Age</th><th>Client ' + (j + 1) + ' State Pension</th><th>Client ' + (j + 1) + ' Other Pension</th><th>Client ' + (j + 1) + ' Salary</th><th>Client ' + (j + 1) + ' Other Income</th><th>Client ' + (j + 1) + ' Contribution</th>';
          }
        html_table += '<th>Total Required Drawdown</th><th>Fund Before Drawdown</th><th>Total Drawdown</th><th>Total Fund Value</th></tr>';
        for (var i = 0; i < json_data.length; i++) {
          html_table += '<tr>';
          html_table += '<td>' + json_data[i].year + '</td>';

          for (var j = 0; j < json_data[i].clients.length; j++) {
            html_table += '<td>' + json_data[i].clients[j].age + '</td>';
            html_table += '<td>' + json_data[i].clients[j].statePension + '</td>';
            html_table += '<td>' + json_data[i].clients[j].otherPension + '</td>';
            html_table += '<td>' + json_data[i].clients[j].salary + '</td>';
            html_table += '<td>' + json_data[i].clients[j].otherIncome + '</td>';
            html_table += '<td>' + json_data[i].clients[j].contribution + '</td>';
          }

          html_table += '<td>' + json_data[i].totalRequiredDrawdown + '</td>';
          html_table += '<td>' + json_data[i].fundBeforeDrawdown + '</td>';
          html_table += '<td>' + json_data[i].totalDrawdown + '</td>';
          html_table += '<td>' + json_data[i].totalFundValue + '</td>';
          html_table += '</tr>';
        }
        html_table += '</tbody></table>';
        $('#json-output').html(html_table);
      } else {
        var error_message = response.data.message;
        $('#json-output').html('<p>' + error_message + '</p>');
      }
    });


  });


  // Handle image retrieval
  $('#image-button').on('click', function (event) {
    var api_url = external_api_params.api_url;
    // Access the security nonce value from the localized object
    var security = external_api_params.security_image;


    // Gather input values
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
      client1.otherIncome = $('#client1OtherIncome').val();
    }

    // Check if adhocTransactions are filled:
    if ($('#client1AdhocTransactionAge').val() && $('#client1AdhocTransactionAmount').val()) {
      client1.adhocTransactions = [{
        age: $('#client1AdhocTransactionAge').val(),
        amount: $('#client1AdhocTransactionAmount').val()
      }];
    }

    var clients = [client1];

    var clientToggle = document.getElementById('NumberOfClients');
    if (clientToggle.value == 2) {
      var client2 = {
        age: $('#client2Age').val(),
        retirementAge: $('#client2RetirementAge').val(),
        statePensionAmount: $('#client2StatePensionAmount').val(),
        statePensionAge: $('#client2StatePensionAge').val(),
        retirementIncomeLevel: $('#client2RetirementIncomeLevel').val()
      };

      // Check if fullSalaryAmount is filled
      var fullSalaryAmountClient2 = $('#client2FullSalaryAmount').val();
      if (fullSalaryAmountClient2) {
        client2.salaryDetails = {
          fullSalaryAmount: fullSalaryAmountClient2
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
        client2.otherIncome = $('#client2OtherIncome').val();
      }

      // Check if adhocTransactions are filled:
      if ($('#client2AdhocTransactionAge').val() && $('#client2AdhocTransactionAmount').val()) {
        client2.adhocTransactions = [{
          age: $('#client2AdhocTransactionAge').val(),
          amount: $('#client2AdhocTransactionAmount').val()
        }];
      }

      clients.push(client2);
    }


    // Prepare the data for the API request
    var data = {
      action: 'external_api_image_request',
      security: security,
      pensionInputData: {
        numberOfYears: numberOfYears,
        indexation: indexation,
        retirementPot: retirementPot,
        investmentGrowth: investmentGrowth,
        clients: clients
      }
    }

    $.post(api_url, data, function (response) {
      if (response.success) {
        var image_data = response.data.image_data;
        var image_url = 'data:image/png;base64,' + image_data;
        $('#image-output').html('<img src="' + image_url + '" alt="External API Image">');
      } else {
        console.log(response.data.message);
      }
    });
  });


  // Handle PDF retrieval
  $('#pdf-button').on('click', function (event) {
    //event.preventDefault();

    var api_url = external_api_params.api_url;
    // Access the security nonce value from the localized object
    var security = external_api_params.security_pdf;


    // Gather input values
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
      client1.otherIncome = $('#client1OtherIncome').val();
    }

    // Check if adhocTransactions are filled:
    if ($('#client1AdhocTransactionAge').val() && $('#client1AdhocTransactionAmount').val()) {
      client1.adhocTransactions = [{
        age: $('#client1AdhocTransactionAge').val(),
        amount: $('#client1AdhocTransactionAmount').val()
      }];
    }

    var clients = [client1];

    var clientToggle = document.getElementById('NumberOfClients');
    if (clientToggle.value == 2) {
      var client2 = {
        age: $('#client2Age').val(),
        retirementAge: $('#client2RetirementAge').val(),
        statePensionAmount: $('#client2StatePensionAmount').val(),
        statePensionAge: $('#client2StatePensionAge').val(),
        retirementIncomeLevel: $('#client2RetirementIncomeLevel').val()
      };

      // Check if fullSalaryAmount is filled
      var fullSalaryAmountClient2 = $('#client2FullSalaryAmount').val();
      if (fullSalaryAmountClient2) {
        client2.salaryDetails = {
          fullSalaryAmount: fullSalaryAmountClient2
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
        client2.otherIncome = $('#client2OtherIncome').val();
      }

      // Check if adhocTransactions are filled:
      if ($('#client2AdhocTransactionAge').val() && $('#client2AdhocTransactionAmount').val()) {
        client2.adhocTransactions = [{
          age: $('#client2AdhocTransactionAge').val(),
          amount: $('#client2AdhocTransactionAmount').val()
        }];
      }

      clients.push(client2);
    }


    // Prepare the data for the API request
    var data = {
      action: 'external_api_pdf_request',
      security: security,
      pensionInputData: {
        numberOfYears: numberOfYears,
        indexation: indexation,
        retirementPot: retirementPot,
        investmentGrowth: investmentGrowth,
        clients: clients
      }
    }


    $.post(api_url, data, function (response) {
      if (response.success) {

        var pdf_data = response.data.pdf_data;
        var pdf_url = 'data:application/pdf;base64,' + pdf_data;

        //var blob = new Blob([pdf_data], { type: 'application/pdf' });
        //var url = URL.createObjectURL(blob);

        // Trigger the file download by creating a temporary link and clicking it
        var link = document.createElement('a');
        link.href = pdf_url;
        link.download = 'Report.pdf';
        link.click();

        // Clean up the temporary URL
        URL.revokeObjectURL(pdf_url);
      } else {
        console.log(response.data.message);
      }
    });
    
  });

});
