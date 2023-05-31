<?php
/*
Plugin Name: Retirement Income Planner Plugin
Description: Plugin for sending data to Retirement Income Planner API
Version: 2.6
Author: Mike Dunn & Richard Scott
*/

// Register a shortcode to embed the buttons and output areas
add_shortcode('retirement_income_planner', 'retirement_income_planner_form_shortcode');

// Enqueue the retirement-income-planner.js file and pass API URL as a variable
function retirement_income_planner_enqueue_scripts()
{
    wp_enqueue_script('external-api', plugin_dir_url(__FILE__) . 'retirement-income-planner.js', array('jquery'), '1.0', true);

    // Pass API URL as a variable to the retirement-income-planner.js file
    wp_localize_script('external-api', 'external_api_params', array(
        'api_url' => esc_js(admin_url('admin-ajax.php')),
        'security_json' => esc_js(wp_create_nonce('external-api-json-nonce')),
        'security_image' => esc_js(wp_create_nonce('external-api-image-nonce')),
        'security_pdf' => esc_js(wp_create_nonce('external-api-pdf-nonce')),
    )
    );
}
add_action('wp_enqueue_scripts', 'retirement_income_planner_enqueue_scripts');


function retirement_income_planner_form_shortcode($atts)
{
    ob_start(); // Start output buffering

    // Enqueue jQuery library
    wp_enqueue_script('jquery');

    // Display the buttons
    ?>

    <div id="retirement-income-planner">
        <form id="retirement-income-planner-form">
            <div class="client-toggle-wrap">
                <label for="NumberOfClients">Number of Clients:<span class="saasify-required">*</span></label>
                <div>

                    <label class="switch">

                        <select id="NumberOfClients" value="1">
                            <option value="1" selected>1</option>
                            <option value="2">2</option>
                        </select>
                    </label>

                </div>
            </div>


            <label for="numberOfYears">Number of Years:<span class="saasify-required">*</span></label>
            <input type="text" id="numberOfYears" name="numberOfYears" required><br>

            <label for="indexation">Indexation:<span class="saasify-required">*</span></label>
            <input type="text" id="indexation" name="indexation" required><br>

            <label for="retirementPot">Retirement Pot:<span class="saasify-required">*</span></label>
            <input type="text" id="retirementPot" name="retirementPot" required><br>

            <label for="investmentGrowth">Investment Growth:<span class="saasify-required">*</span></label>
            <input type="text" id="investmentGrowth" name="investmentGrowth" required><br>


            <div id="client-info">
                <div class="client-inputs">

                    <h3>Client 1</h3>
                    <label for="client1Age">Age:<span class="saasify-required">*</span></label>
                    <input type="text" id="client1Age" name="client1Age" required><br>

                    <label for="client1FullSalaryAmount">Full Salary Amount:</label>
                    <input type="text" id="client1FullSalaryAmount" name="client1FullSalaryAmount"><br>

                    <label for="client1PartialRetirementAge">Partial Retirement Age:</label>
                    <input type="text" id="client1PartialRetirementAge" name="client1PartialRetirementAge"><br>

                    <label for="client1PartialRetirementAmount">Partial Retirement Amount:</label>
                    <input type="text" id="client1PartialRetirementAmount" name="client1PartialRetirementAmount"><br>

                    <label for="client1RetirementAge">Retirement Age:<span class="saasify-required">*</span></label>
                    <input type="text" id="client1RetirementAge" name="client1RetirementAge" required><br>

                    <label for="client1StatePensionAmount">State Pension Amount:<span
                            class="saasify-required">*</span></label>
                    <input type="text" id="client1StatePensionAmount" name="client1StatePensionAmount" required><br>

                    <label for="client1StatePensionAge">State Pension Age:<span class="saasify-required">*</span></label>
                    <input type="text" id="client1StatePensionAge" name="client1StatePensionAge" required><br>

                    <label for="client1OtherPensionAge">Other Pension Age:</label>
                    <input type="text" id="client1OtherPensionAge" name="client1OtherPensionAge"><br>

                    <label for="client1OtherPensionAmount">Other Pension Amount:</label>
                    <input type="text" id="client1OtherPensionAmount" name="client1OtherPensionAmount"><br>

                    <label for="client1OtherIncome">Other Income:</label>
                    <input type="text" id="client1OtherIncome" name="client1OtherIncome"><br>

                    <label for="client1RetirementIncomeLevel">Retirement Income Level:<span
                            class="saasify-required">*</span></label>
                    <input type="text" id="client1RetirementIncomeLevel" name="client1RetirementIncomeLevel" required><br>

                    <label for="client1AdhocTransactionAge">Adhoc Transaction Age:</label>
                    <input type="text" id="client1AdhocTransactionAge" name="client1AdhocTransactionAge"><br>

                    <label for="client1AdhocTransactionAmount">Adhoc Transaction Amount:</label>
                    <input type="text" id="client1AdhocTransactionAmount" name="client1AdhocTransactionAmount"><br>

                </div>

                <div class="client-inputs" style="display: none;">

                    <h3>Client 2</h3>
                    <label for="client2Age">Age:<span class="saasify-required">*</span></label>
                    <input type="text" id="client2Age" name="client2Age"><br>

                    <label for="client2FullSalaryAmount">Full Salary Amount:</label>
                    <input type="text" id="client2FullSalaryAmount" name="client2FullSalaryAmount"><br>

                    <label for="client2PartialRetirementAge">Partial Retirement Age:</label>
                    <input type="text" id="client2PartialRetirementAge" name="client2PartialRetirementAge"><br>

                    <label for="client2PartialRetirementAmount">Partial Retirement Amount:</label>
                    <input type="text" id="client2PartialRetirementAmount" name="client2PartialRetirementAmount"><br>

                    <label for="client2RetirementAge">Retirement Age:<span class="saasify-required">*</span></label>
                    <input type="text" id="client2RetirementAge" name="client2RetirementAge"><br>

                    <label for="client2StatePensionAmount">State Pension Amount:<span
                            class="saasify-required">*</span></label>
                    <input type="text" id="client2StatePensionAmount" name="client2StatePensionAmount"><br>

                    <label for="client2StatePensionAge">State Pension Age:<span class="saasify-required">*</span></label>
                    <input type="text" id="client2StatePensionAge" name="client2StatePensionAge"><br>

                    <label for="client2OtherPensionAge">Other Pension Age:</label>
                    <input type="text" id="client2OtherPensionAge" name="client2OtherPensionAge"><br>

                    <label for="client2OtherPensionAmount">Other Pension Amount:</label>
                    <input type="text" id="client2OtherPensionAmount" name="client2OtherPensionAmount"><br>

                    <label for="client2OtherIncome">Other Income:</label>
                    <input type="text" id="client2OtherIncome" name="client2OtherIncome"><br>

                    <label for="client2RetirementIncomeLevel">Retirement Income Level:<span
                            class="saasify-required">*</span></label>
                    <input type="text" id="client2RetirementIncomeLevel" name="client2RetirementIncomeLevel"><br>

                    <label for="client2AdhocTransactionAge">Adhoc Transaction Age:</label>
                    <input type="text" id="client2AdhocTransactionAge" name="client2AdhocTransactionAge"><br>

                    <label for="client2AdhocTransactionAmount">Adhoc Transaction Amount:</label>
                    <input type="text" id="client2AdhocTransactionAmount" name="client2AdhocTransactionAmount"><br>
                </div>
            </div>

            <button type="submit" id="json-button">Show calculated data</button>
            <button type="submit" id="image-button">Preview Chart</button>
            <button type="submit" id="pdf-button">Download Report</button>
        </form>


        <div id="image-output"></div>
        <div id="json-output"></div>
        

    </div>


    <?php

    return ob_get_clean(); // Return the buffered output
}

// AJAX handler for the JSON data request
add_action('wp_ajax_external_api_json_request', 'handle_external_api_json_request');
add_action('wp_ajax_nopriv_external_api_json_request', 'handle_external_api_json_request');
function handle_external_api_json_request()
{
    check_ajax_referer('external-api-json-nonce', 'security');

    // Get the pension input data from the AJAX request
    $pension_input_data = $_POST['pensionInputData'];


    // API endpoint URL
    $api_url = 'http://localhost:5001/api/RetirementIncomePlanner/RequestOutputData';

    // Prepare the data to be sent to the external API
    $data = array(
        'pensionInputData' => $pension_input_data
    );

    //wp_send_json_error(array('message' => json_encode($data)));

    $response = wp_remote_post(
        $api_url,
        array(
            'headers' => array('Content-Type' => 'application/json'),
            'body' => json_encode($data),
            'timeout' => 30
        )
    );

    if (is_wp_error($response) || wp_remote_retrieve_response_code($response) !== 200) {
        $error_message = 'Failed to fetch JSON data.';
        wp_send_json_error(array('message' => $error_message));
    } else {
        $json_data = wp_remote_retrieve_body($response);
        wp_send_json_success(array('json_data' => $json_data));
    }

    wp_die();
}

// AJAX handler for the image request
add_action('wp_ajax_external_api_image_request', 'handle_external_api_image_request');
add_action('wp_ajax_nopriv_external_api_image_request', 'handle_external_api_image_request');
function handle_external_api_image_request()
{
    check_ajax_referer('external-api-image-nonce', 'security');

    // Get the pension input data from the AJAX request
    $pension_input_data = $_POST['pensionInputData'];

    // API endpoint URL and data
    $api_url = 'http://localhost:5001/api/RetirementIncomePlanner/RequestChartImage';
    $data = array(
        'pensionInputData' => $pension_input_data,
        'pensionChartColors' => array(
            'totalDrawdownColor' => '#305d7a',
            'statePensionPrimaryColor' => '#746aa3',
            'statePensionSecondaryColor' => '#c9c0e7',
            'otherPensionPrimaryColor' => '#ca6ca2',
            'otherPensionSecondaryColor' => '#f2bbda',
            'salaryPrimaryColor' => '#ff7d76',
            'salarySecondaryColor' => '#ffc1b9',
            'otherIncomePrimaryColor' => '#ffb13e',
            'otherIncomeSecondaryColor' => '#ffd29f',
            'totalFundValueColor' => '#000000'
        ),
        'imageSize' => array(
            'width' => 800,
            'height' => 600
        )
    );

    $response = wp_remote_post(
        $api_url,
        array(
            'headers' => array('Content-Type' => 'application/json'),
            'body' => json_encode($data),
            'timeout' => 30
        )
    );

    if (is_wp_error($response) || wp_remote_retrieve_response_code($response) !== 200) {
        $error_message = 'Failed to fetch image.';
        wp_send_json_error(array('message' => $error_message));
    } else {
        $image_data = wp_remote_retrieve_body($response);
        $encoded_image_data = base64_encode($image_data);
        wp_send_json_success(array('image_data' => $encoded_image_data));
    }

    wp_die();
}

// AJAX handler for the PDF request
add_action('wp_ajax_external_api_pdf_request', 'handle_external_api_pdf_request');
add_action('wp_ajax_nopriv_external_api_pdf_request', 'handle_external_api_pdf_request');
function handle_external_api_pdf_request()
{
    check_ajax_referer('external-api-pdf-nonce', 'security');

    // Get the pension input data from the AJAX request
    $pension_input_data = $_POST['pensionInputData'];

    // API endpoint URL and data
    $api_url = 'http://localhost:5001/api/RetirementIncomePlanner/RequestReportPDF';
    $data = array(
        'pensionInputData' => $pension_input_data,
        'pensionChartColors' => array(
            'totalDrawdownColor' => '#305d7a',
            'statePensionPrimaryColor' => '#746aa3',
            'statePensionSecondaryColor' => '#c9c0e7',
            'otherPensionPrimaryColor' => '#ca6ca2',
            'otherPensionSecondaryColor' => '#f2bbda',
            'salaryPrimaryColor' => '#ff7d76',
            'salarySecondaryColor' => '#ffc1b9',
            'otherIncomePrimaryColor' => '#ffb13e',
            'otherIncomeSecondaryColor' => '#ffd29f',
            'totalFundValueColor' => '#000000'
        )
    );

    $response = wp_remote_post(
        $api_url,
        array(
            'headers' => array('Content-Type' => 'application/json'),
            'body' => json_encode($data),
            'timeout' => 30
        )
    );

    if (is_wp_error($response) || wp_remote_retrieve_response_code($response) !== 200) {
        $error_message = 'Failed to fetch PDF document.';
        wp_send_json_error(array('message' => $error_message));
    } else {
        $pdf_data = wp_remote_retrieve_body($response);

        
        $encoded_pdf_data = base64_encode($pdf_data);
        wp_send_json_success(array('pdf_data' => $encoded_pdf_data));

        
    }

    wp_die();
}