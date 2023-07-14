function validateForm() {
  var inputs = document.getElementsByTagName('input');
  var isValid = true;

  for (var i = 0; i < inputs.length; i++) {
    if (inputs[i].hasAttribute('required') && inputs[i].value === '') {
      isValid = false;
      //inputs[i].classList.add('error');
    } //else {
    //inputs[i].classList.remove('error');
    //}
  }

  return isValid;
}


// Function to add a new contribution
function addContribution(prefix) {

  var removeButtonClassName = prefix + 'RemoveContribution';
  var contributionsContainer = document.getElementById(prefix + 'ContributionsContainer');

  // Create a new contribution tr
  var newContribution = document.createElement('tr');
  newContribution.className = prefix + 'Contribution';

  // Add the HTML for the contribution fields
  newContribution.innerHTML = `
  <td class="form-field">
  <input type="text" name="${prefix}AdhocTransactionAge[]" class="adhocTransactionAge">
  </td>
  <td class="form-field">
  <input type="text" name="${prefix}AdhocTransactionAmount[]" class="adhocTransactionAmount">
  </td>
  <td class="remove-button-column">
  <button type="button" class="${removeButtonClassName}">X</button>
  </td>
  `;

  // Append the new contribution to the container
  contributionsContainer.appendChild(newContribution);

  // Add event listener to remove button
  var removeButton = newContribution.querySelector(`.${removeButtonClassName}`);
  if (removeButton !== null) {
    removeButton.addEventListener('click', removeContribution);
  }
}

// Function to remove a contribution
function removeContribution(event) {
  var contribution = event.target.parentNode.parentNode;
  contribution.parentNode.removeChild(contribution);
}

// Add event listener to the "Add Contribution" button for client 1
var client1AddButton = document.getElementById('client1AddContribution');
if (client1AddButton !== null) {
  client1AddButton.addEventListener('click', function () {
    addContribution('client1');
  });
}

// Add event listener to the "Add Contribution" button for client 2
var client2AddButton = document.getElementById('client2AddContribution');
if (client2AddButton !== null) {
  client2AddButton.addEventListener('click', function () {
    addContribution('client2');
  });
}

//Code to hide/display Client 2 based on drop-down:
var numClientsDrop = document.getElementById("NumberOfClients");
if (numClientsDrop !== null) {
  numClientsDrop.addEventListener("change", function () {
    var clientInputs2 = document.getElementById("client2input");

    if (numClientsDrop.value == 2) {
      clientInputs2.style.display = "block";
      setClientFieldsRequired(true, 2);
    } else {
      clientInputs2.style.display = "none";
      setClientFieldsRequired(false, 2);
    }
  });
}

function setClientFieldsRequired(setRequired, clientNumber) {
  document.getElementById(`client${clientNumber}Age`).required = setRequired;
  document.getElementById(`client${clientNumber}RetirementAge`).required = setRequired;
  document.getElementById(`client${clientNumber}StatePensionAmount`).required = setRequired;
  document.getElementById(`client${clientNumber}StatePensionAge`).required = setRequired;
  document.getElementById(`client${clientNumber}RetirementIncomeLevel`).required = setRequired;
}

// Get the value of a parameter from a serialized form string
function getParameterValue(serializedString, parameterName) {
  var parameters = serializedString.split('&');
  for (var i = 0; i < parameters.length; i++) {
    var parameter = parameters[i].split('=');
    if (decodeURIComponent(parameter[0]) === parameterName) {
      return decodeURIComponent(parameter[1]);
    }
  }
  return '';
}

jQuery(document).ready(function ($) {
  // Initialize the color picker
  $('.color-picker').spectrum({
    preferredFormat: 'hex',
    showInput: true,
    showPalette: true,
    palette: [
      ['#000000', '#ffffff', '#ff0000', '#00ff00', '#0000ff'],
      ['#ffff00', '#ff00ff', '#00ffff', '#ff9900', '#9900ff'],
      ['#305d7a','#746aa3','#c9c0e7','#ca6ca2', '#f2bbda'],
      ['#ff7d76','#ffc1b9','#ffb13e', '#ffd29f']
    ]
  });

  // Store the initial form values
  var initialFormValues = $('#custom-user-meta-form').serialize();

  // Handle cancel button click event
  $('input[name="cancel_custom_user_meta"]').on('click', function (e) {
    e.preventDefault();

    // Reset the form fields to their initial values
    $('.color-picker').each(function () {
      var fieldName = $(this).attr('name');
      var initialValue = getParameterValue(initialFormValues, fieldName);
      $(this).spectrum('set', initialValue);
    });
  });

  // Handle reset button click event
  $('input[name="reset_custom_user_meta"]').on('click', function (e) {
    e.preventDefault();

    // Reset the form fields to the values from chartColors
    $('.color-picker').each(function () {
      var fieldName = $(this).attr('name');
      var resetValue = chartColors[fieldName];
      $(this).spectrum('set', resetValue);
    });
  });


  $('#income-planner-form').submit(function (e) {
    e.preventDefault(); // Prevent form submission
  });

  // Handle JSON data retrieval
  $('#json-button').on('click', function (event) {

    if (!validateForm()) {
      $('#json-output').html('<p>' + 'Please ensure all required fields are completed.' + '</p>');
      return;
    }

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

    if (!validateForm()) {
      $('#json-output').html('<p>' + 'Please ensure all required fields are completed.' + '</p>');
      return;
    }

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
        $('#image-output').html('<img width="100%" src="' + image_url + '" alt="Chart Preview">');
        $('#json-output').html('');
      } else {
        var error_message = response.data.message;
        $('#json-output').html('<p>' + error_message + '</p>');
      }
    });
  });


  // Handle PDF retrieval
  $('#pdf-button').on('click', function (event) {

    if (!validateForm()) {
      $('#json-output').html('<p>' + 'Please ensure all required fields are completed.' + '</p>');
      return;
    }

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

    var element = $(`#client${clientNumber}ContributionsContainer`);

    var adhocArray = [];

    // Iterate over each AdHoc field within the client
    $(element).find('.adhocTransactionAge').each(function (ageIndex, ageElement) {
      var age = $(ageElement).val();
      var amount = $(element).find('.adhocTransactionAmount').eq(ageIndex).val();
      if (age && amount) {
        adhocArray.push({ age: age, amount: amount });
      }
    });

    if (adhocArray) {
      client.adhocTransactions = adhocArray;
    }

    return client;
  }

});

