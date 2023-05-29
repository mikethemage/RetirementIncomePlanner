jQuery(document).ready(function($) {
    $('#api-integration form').submit(function(e) {
      e.preventDefault(); // Prevent form submission
  
      // Get input values
      var numberOfYears = $('#numberOfYears').val();
      var indexation = $('#indexation').val();
      var retirementPot = $('#retirementPot').val();
      var clientNumber = $('#clientNumber').val();
      var age = $('#age').val();
      var fullSalaryAmount = $('#fullSalaryAmount').val();
      var partialRetirementAge = $('#partialRetirementAge').val();
      var partialRetirementAmount = $('#partialRetirementAmount').val();
      var retirementAge = $('#retirementAge').val();
      var statePensionAmount = $('#statePensionAmount').val();
      var statePensionAge = $('#statePensionAge').val();
      var otherPensionAge = $('#otherPensionAge').val();
      var otherPensionAmount = $('#otherPensionAmount').val();
      var otherIncome = $('#otherIncome').val();
      var retirementIncomeLevel = $('#retirementIncomeLevel').val();
  
      // Prepare the data for the API request
      var requestData = {
        numberOfYears: parseInt(numberOfYears),
        indexation: parseFloat(indexation),
        retirementPot: parseInt(retirementPot),
        investmentGrowth: 0.03,
        clientNumber: parseInt(clientNumber),
        age: parseInt(age),
        salaryDetails: {
          fullSalaryAmount: parseInt(fullSalaryAmount),
          partialRetirementDetails: {
            age: parseInt(partialRetirementAge),
            amount: parseInt(partialRetirementAmount)
          }
        },
        retirementAge: parseInt(retirementAge),
        statePensionAmount: parseInt(statePensionAmount),
        statePensionAge: parseInt(statePensionAge),
        otherPensionDetails: {
          age: parseInt(otherPensionAge),
          amount: parseInt(otherPensionAmount)
        },
        otherIncome: parseInt(otherIncome),
        retirementIncomeLevel: parseInt(retirementIncomeLevel)
      };
  
      // Make the AJAX request to the API
      $.ajax({
        url: apiIntegrationAjax.ajaxurl, // Use the localized AJAX URL
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(requestData),
        success: function(response) {
          console.log(response); // For debugging purposes
  
          // Extract the result value from the response
          var resultValue = response.result;
  
          // Display the result
          $('#api-result').text('Result: ' + resultValue);
        },
        error: function(xhr, status, error) {
          console.log(xhr.responseText); // For debugging purposes
          $('#api-result').text('Error: ' + error);
        }
      });
    });
  });
  