<?php
/*
Plugin Name: Retirement Income Planner Plugin
Description: Plugin for sending data to Retirement Income Planner API
Version: 1.0
Author: Your Name
*/

// Enqueue scripts and localize the AJAX URL
function retirement_income_planner_enqueue_scripts() {
  // Enqueue the JavaScript file
  wp_enqueue_script('retirement-income-planner-script', plugin_dir_url(__FILE__) . 'retirement-income-planner.js', array('jquery'), '1.0', true);

  // Localize the script and pass the AJAX URL
  wp_localize_script('retirement-income-planner-script', 'retirementIncomePlannerAjax', array(
    'ajaxurl' => admin_url('admin-ajax.php')
  ));
}
add_action('wp_enqueue_scripts', 'retirement_income_planner_enqueue_scripts');

// Handle the AJAX request and send data to the API endpoint
function retirement_income_planner_proxy_request() {
  if (isset($_POST['data'])) {
    $jsonData = stripslashes($_POST['data']);
    $apiUrl = 'http://localhost:5001/api/RetirementIncomePlanner/RequestOutputData';

    $curl = curl_init($apiUrl);
    curl_setopt($curl, CURLOPT_CUSTOMREQUEST, 'POST');
    curl_setopt($curl, CURLOPT_TIMEOUT, 60);
    curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($curl, CURLOPT_HTTP_VERSION, CURL_HTTP_VERSION_1_0);
    curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, false);
    curl_setopt($curl, CURLOPT_POSTFIELDS, $jsonData);
    curl_setopt($curl, CURLOPT_HTTPHEADER, array('Content-Type: application/json')); // Set the content type header
    $response = curl_exec($curl);

    if ($response === false) {
      die('Error: ' . curl_error($curl));
    }

    curl_close($curl);

    wp_send_json($response);
  }
}
add_action('wp_ajax_retirement_income_planner_proxy_request', 'retirement_income_planner_proxy_request');
add_action('wp_ajax_nopriv_retirement_income_planner_proxy_request', 'retirement_income_planner_proxy_request');

// Shortcode for the Retirement Income Planner form
function retirement_income_planner_form_shortcode() {
  ob_start();
  ?>
  <div id="retirement-income-planner">
    <form id="retirement-income-planner-form">
      <label for="numberOfYears">Number of Years:</label>
      <input type="text" id="numberOfYears" name="numberOfYears"><br>

      <label for="indexation">Indexation:</label>
      <input type="text" id="indexation" name="indexation"><br>

      <label for="retirementPot">Retirement Pot:</label>
      <input type="text" id="retirementPot" name="retirementPot"><br>

      <label for="investmentGrowth">Investment Growth:</label>
      <input type="text" id="investmentGrowth" name="investmentGrowth"><br>

      <h3>Client 1</h3>
      <label for="client1Age">Age:</label>
      <input type="text" id="client1Age" name="client1Age"><br>

      <label for="client1FullSalaryAmount">Full Salary Amount:</label>
      <input type="text" id="client1FullSalaryAmount" name="client1FullSalaryAmount"><br>

      <label for="client1PartialRetirementAge">Partial Retirement Age:</label>
      <input type="text" id="client1PartialRetirementAge" name="client1PartialRetirementAge"><br>

      <label for="client1PartialRetirementAmount">Partial Retirement Amount:</label>
      <input type="text" id="client1PartialRetirementAmount" name="client1PartialRetirementAmount"><br>

      <label for="client1RetirementAge">Retirement Age:</label>
      <input type="text" id="client1RetirementAge" name="client1RetirementAge"><br>

      <label for="client1StatePensionAmount">State Pension Amount:</label>
      <input type="text" id="client1StatePensionAmount" name="client1StatePensionAmount"><br>

      <label for="client1StatePensionAge">State Pension Age:</label>
      <input type="text" id="client1StatePensionAge" name="client1StatePensionAge"><br>

      <label for="client1OtherPensionAge">Other Pension Age:</label>
      <input type="text" id="client1OtherPensionAge" name="client1OtherPensionAge"><br>

      <label for="client1OtherPensionAmount">Other Pension Amount:</label>
      <input type="text" id="client1OtherPensionAmount" name="client1OtherPensionAmount"><br>

      <label for="client1OtherIncome">Other Income:</label>
      <input type="text" id="client1OtherIncome" name="client1OtherIncome"><br>

      <label for="client1RetirementIncomeLevel">Retirement Income Level:</label>
      <input type="text" id="client1RetirementIncomeLevel" name="client1RetirementIncomeLevel"><br>

      <label for="client1AdhocTransactionAge">Adhoc Transaction Age:</label>
      <input type="text" id="client1AdhocTransactionAge" name="client1AdhocTransactionAge"><br>

      <label for="client1AdhocTransactionAmount">Adhoc Transaction Amount:</label>
      <input type="text" id="client1AdhocTransactionAmount" name="client1AdhocTransactionAmount"><br>

      <h3>Client 2</h3>
      <label for="client2Age">Age:</label>
      <input type="text" id="client2Age" name="client2Age"><br>

      <label for="client2FullSalaryAmount">Full Salary Amount:</label>
      <input type="text" id="client2FullSalaryAmount" name="client2FullSalaryAmount"><br>

      <label for="client2PartialRetirementAge">Partial Retirement Age:</label>
      <input type="text" id="client2PartialRetirementAge" name="client2PartialRetirementAge"><br>

      <label for="client2PartialRetirementAmount">Partial Retirement Amount:</label>
      <input type="text" id="client2PartialRetirementAmount" name="client2PartialRetirementAmount"><br>

      <label for="client2RetirementAge">Retirement Age:</label>
      <input type="text" id="client2RetirementAge" name="client2RetirementAge"><br>

      <label for="client2StatePensionAmount">State Pension Amount:</label>
      <input type="text" id="client2StatePensionAmount" name="client2StatePensionAmount"><br>

      <label for="client2StatePensionAge">State Pension Age:</label>
      <input type="text" id="client2StatePensionAge" name="client2StatePensionAge"><br>

      <label for="client2OtherPensionAge">Other Pension Age:</label>
      <input type="text" id="client2OtherPensionAge" name="client2OtherPensionAge"><br>

      <label for="client2OtherPensionAmount">Other Pension Amount:</label>
      <input type="text" id="client2OtherPensionAmount" name="client2OtherPensionAmount"><br>

      <label for="client2OtherIncome">Other Income:</label>
      <input type="text" id="client2OtherIncome" name="client2OtherIncome"><br>

      <label for="client2RetirementIncomeLevel">Retirement Income Level:</label>
      <input type="text" id="client2RetirementIncomeLevel" name="client2RetirementIncomeLevel"><br>

      <label for="client2AdhocTransactionAge">Adhoc Transaction Age:</label>
      <input type="text" id="client2AdhocTransactionAge" name="client2AdhocTransactionAge"><br>

      <label for="client2AdhocTransactionAmount">Adhoc Transaction Amount:</label>
      <input type="text" id="client2AdhocTransactionAmount" name="client2AdhocTransactionAmount"><br>

      <button type="submit">Submit</button>
    </form>

    <div id="retirement-income-planner-result"></div>
  </div>
  <?php
  return ob_get_clean();
}
add_shortcode('retirement_income_planner', 'retirement_income_planner_form_shortcode');
