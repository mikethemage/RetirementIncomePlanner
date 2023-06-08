// Function to add a new contribution
function client1AddContribution() {
  var contributionsContainer = document.getElementById('client1ContributionsContainer');

  // Create a new contribution div
  var newContribution = document.createElement('div');
  newContribution.className = 'client1Contribution';

  // Add the HTML for the contribution fields
  newContribution.innerHTML = `
  <p class="form-field">
  <label for="client1AdhocTransactionAge">Adhoc Transaction Age:</label>
  <input type="text" name="client1AdhocTransactionAge[]" class="adhocTransactionAge">
  </p>
  <p class="form-field">
  <label for="client1AdhocTransactionAmount">Adhoc Transaction Amount:</label>
  <input type="text" name="client1AdhocTransactionAmount[]" class="adhocTransactionAmount">
  </p>
  <button type="button" class="client1RemoveContribution">Remove</button>
  `;

  // Append the new contribution to the container
  contributionsContainer.appendChild(newContribution);

  // Add event listener to remove button
  var removeButton = newContribution.querySelector('.client1RemoveContribution');
  removeButton.addEventListener('click', removeContribution);
}

function client2AddContribution() {
  var contributionsContainer = document.getElementById('client2ContributionsContainer');

  // Create a new contribution div
  var newContribution = document.createElement('div');
  newContribution.className = 'client2Contribution';

  // Add the HTML for the contribution fields
  newContribution.innerHTML = `
  <p class="form-field">
  <label for="client2AdhocTransactionAge">Adhoc Transaction Age:</label>
  <input type="text" name="client2AdhocTransactionAge[]" class="adhocTransactionAge">
  </p>
  <p class="form-field">
  <label for="client2AdhocTransactionAmount">Adhoc Transaction Amount:</label>
  <input type="text" name="client2AdhocTransactionAmount[]" class="adhocTransactionAmount">
  </p>
  <button type="button" class="client2RemoveContribution">Remove</button>
  `;

  // Append the new contribution to the container
  contributionsContainer.appendChild(newContribution);

  // Add event listener to remove button
  var removeButton = newContribution.querySelector('.client2RemoveContribution');
  removeButton.addEventListener('click', removeContribution);
}

// Function to remove a contribution
function removeContribution(event) {
  var contribution = event.target.parentNode;
  contribution.parentNode.removeChild(contribution);
}

// Add event listener to the "Add Contribution" button
var addButton = document.getElementById('client1AddContribution');
addButton.addEventListener('click', client1AddContribution);

// Add event listener to the "Add Contribution" button
var addButton = document.getElementById('client2AddContribution');
addButton.addEventListener('click', client2AddContribution);


//Code to hide/display Client 2 based on drop-down:
var numClientsDrop = document.getElementById("NumberOfClients");
var clientInfo = document.getElementById("client-info");

numClientsDrop.addEventListener("change", function () {
  var clientInputs2 = document.querySelector(".client-inputs:nth-child(2)");

  if (numClientsDrop.value == 2) {
    clientInputs2.style.display = "block";
    setClient2FieldsRequired(true);
  } else {
    clientInputs2.style.display = "none";
    setClient2FieldsRequired(false);
  }
});

function setClient2FieldsRequired(setRequired) {
  document.getElementById("client2Age").required = setRequired;
  document.getElementById("client2RetirementAge").required = setRequired;
  document.getElementById("client2StatePensionAmount").required = setRequired;
  document.getElementById("client2StatePensionAge").required = setRequired;
  document.getElementById("client2RetirementIncomeLevel").required = setRequired;
}

jQuery(document).ready(function ($) {

  $('#retirement-income-planner-form').submit(function (e) {
    e.preventDefault(); // Prevent form submission
  });

  // Handle JSON data retrieval
  $('#json-button').on('click', function (event) {
    //var api_url = external_api_params.api_url;
    // Access the security nonce value from the localized object
    var security = external_api_params.security_json;

    // Gather input values
    var pensionInputData = gatherInputData();

    // Prepare the data for the API request
    var data = {
      action: 'external_api_json_request',
      security: security,
      pensionInputData: pensionInputData
    }

    $.post(external_api_params.api_url, data, function (response) {
      if (response.success) {

        var json_data = JSON.parse(response.data.json_data);

        var html_table = '<table><tbody>';
        // Generate HTML table from the JSON data
        // Modify this section according to your JSON structure
        html_table += '<tr><th>Year</th>';

        for (var j = 0; j < json_data[0].clients.length; j++) {
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
    //var api_url = external_api_params.api_url;
    // Access the security nonce value from the localized object
    var security = external_api_params.security_image;

    // Gather input values
    var pensionInputData = gatherInputData();

    // Prepare the data for the API request
    var data = {
      action: 'external_api_image_request',
      security: security,
      pensionInputData: pensionInputData
    }

    $.post(external_api_params.api_url, data, function (response) {
      if (response.success) {
        var image_data = response.data.image_data;
        var image_url = 'data:image/png;base64,' + image_data;
        $('#image-output').html('<img src="' + image_url + '" alt="External API Image">');
        $('#json-output').html('');
      } else {
        var error_message = response.data.message;
        $('#json-output').html('<p>' + error_message + '</p>');
      }
    });
  });


  // Handle PDF retrieval
  $('#pdf-button').on('click', function (event) {

    //var api_url = external_api_params.api_url;
    // Access the security nonce value from the localized object
    var security = external_api_params.security_pdf;

    // Gather input values
    var pensionInputData = gatherInputData();

    // Prepare the data for the API request
    var data = {
      action: 'external_api_pdf_request',
      security: security,
      pensionInputData: pensionInputData
    }

    $.post(external_api_params.api_url, data, function (response) {
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
        //console.log(response.data.message);
        var error_message = response.data.message;
        $('#json-output').html('<p>' + error_message + '</p>');
      }
    });

  });



  function gatherInputData() {
    var numberOfYears = $('#numberOfYears').val();
    var indexationPercentage = $('#indexation').val();
    var retirementPot = $('#retirementPot').val();
    var investmentGrowthPercentage = $('#investmentGrowth').val();

    var indexation = parseFloat(indexationPercentage) / 100;
    var investmentGrowth = parseFloat(investmentGrowthPercentage) / 100;

    var numberOfClients = parseInt($('#NumberOfClients').val());
    var clients = [];

    for (var i = 1; i <= numberOfClients; i++) {
      var client = gatherClientInputData(i);
      clients.push(client);
    }

    var inputData = {
      numberOfYears: numberOfYears,
      indexation: indexation,
      retirementPot: retirementPot,
      investmentGrowth: investmentGrowth,
      clients: clients
    };

    return inputData;
  }

  function gatherClientInputData(clientNumber) {
    var client = {
      age: $(`#client${clientNumber}Age`).val(),
      retirementAge: $(`#client${clientNumber}RetirementAge`).val(),
      statePensionAmount: $(`#client${clientNumber}StatePensionAmount`).val(),
      statePensionAge: $(`#client${clientNumber}StatePensionAge`).val(),
      retirementIncomeLevel: $(`#client${clientNumber}RetirementIncomeLevel`).val()
    };

    var fullSalaryAmount = $(`#client${clientNumber}FullSalaryAmount`).val();
    if (fullSalaryAmount) {
      client.salaryDetails = {
        fullSalaryAmount: fullSalaryAmount
      };

      var partialRetirementAge = $(`#client${clientNumber}PartialRetirementAge`).val();
      var partialRetirementAmount = $(`#client${clientNumber}PartialRetirementAmount`).val();
      if (partialRetirementAge && partialRetirementAmount) {
        client.salaryDetails.partialRetirementDetails = {
          age: partialRetirementAge,
          amount: partialRetirementAmount
        };
      }
    }

    var otherPensionAge = $(`#client${clientNumber}OtherPensionAge`).val();
    var otherPensionAmount = $(`#client${clientNumber}OtherPensionAmount`).val();
    if (otherPensionAge && otherPensionAmount) {
      client.otherPensionDetails = {
        age: otherPensionAge,
        amount: otherPensionAmount
      };
    }

    var otherIncome = $(`#client${clientNumber}OtherIncome`).val();
    if (otherIncome) {
      client.otherIncome = otherIncome;
    }

    //oldcode:
    /*
    var adhocTransactionAge = $(`#client${clientNumber}AdhocTransactionAge`).val();
    var adhocTransactionAmount = $(`#client${clientNumber}AdhocTransactionAmount`).val();
    if (adhocTransactionAge && adhocTransactionAmount) {
      client.adhocTransactions = [{
        age: adhocTransactionAge,
        amount: adhocTransactionAmount
      }];
    }
    */

    //New code:
    var element=$(`#client${clientNumber}ContributionsContainer`);
    
    var adhocArray = [];

    // Iterate over each AdHoc field within the client
    $(element).find('.adhocTransactionAge').each(function(ageIndex, ageElement) {
      var age = $(ageElement).val();
      var amount = $(element).find('.adhocTransactionAmount').eq(ageIndex).val();
      if(age && amount)
      {
        adhocArray.push({ age: age, amount: amount });
      }
    });

    if(adhocArray)
    {
      client.adhocTransactions = adhocArray;
    }    
    

    return client;
  }

});