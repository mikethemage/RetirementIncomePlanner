<?php
/*
Plugin Name: API Integration Plugin2
Description: Plugin for API integration
Version: 1.0
Author: Your Name
*/

// Enqueue scripts and localize the AJAX URL
function api_integration_enqueue_scripts() {
  // Enqueue the JavaScript file
  wp_enqueue_script('api-integration-script', plugin_dir_url(__FILE__) . 'api-integration2.js', array('jquery'), '1.0', true);

  // Localize the script and pass the AJAX URL
  wp_localize_script('api-integration-script', 'apiIntegrationAjax', array(
    'ajaxurl' => admin_url('admin-ajax.php')
  ));
}
add_action('wp_enqueue_scripts', 'api_integration_enqueue_scripts');

// Handle the AJAX request and proxy to the API
function api_integration_proxy_request() {
  if (isset($_POST['data'])) {
    $jsonData = stripslashes($_POST['data']);
    $apiUrl = 'http://localhost:5000/api/Addition';

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
add_action('wp_ajax_api_integration_proxy_request', 'api_integration_proxy_request');
add_action('wp_ajax_nopriv_api_integration_proxy_request', 'api_integration_proxy_request');

// Shortcode for the API Integration form
function api_integration_form_shortcode() {
  ob_start();
  ?>
  <div id="api-integration">
    <form id="api-form">
      <label for="operand1">Number 1:</label>
      <input type="text" id="operand1" name="operand1"><br>

      <label for="operand2">Number 2:</label>
      <input type="text" id="operand2" name="operand2"><br>

      <button type="submit">Calculate</button>
    </form>

    <div id="api-result"></div>
  </div>
  <?php
  return ob_get_clean();
}
add_shortcode('api_integration2', 'api_integration_form_shortcode');
