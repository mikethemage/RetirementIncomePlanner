<?php
/*
Plugin Name: Retirement Income Planner Plugin
Description: Plugin for sending data to Retirement Income Planner API
Version: 3.14
Author: Mike Dunn & Richard Scott
*/

// Exit if accessed directly
if (!defined('ABSPATH')) {
    exit;
}

// Define the API URL constant:
define('API_URL', 'http://localhost:5001/api/RetirementIncomePlanner/');

// Register shortcodes to embed the buttons and output areas:
add_shortcode('income_planner_form_header', 'income_planner_form_header_shortcode');
add_shortcode('income_planner_client_1_input', 'income_planner_client_1_input_shortcode');
add_shortcode('income_planner_client_2_input', 'income_planner_client_2_input_shortcode');
add_shortcode('income_planner_form_footer', 'income_planner_form_footer_shortcode');

// Enqueue necessary scripts and styles for Colour picker and data:
function custom_user_meta_enqueue_scripts()
{
    // Enqueue spectrum.js script
    wp_enqueue_script('spectrum-script', plugin_dir_url(__FILE__) . 'spectrum.js', array('jquery'), '1.8.1', true);

    // Enqueue spectrum.css stylesheet
    wp_enqueue_style('spectrum-style', plugin_dir_url(__FILE__) . 'spectrum.css', array(), '1.8.1');

    //enqure stylesheet:
    wp_enqueue_style( 'plugin-custom-styles', plugin_dir_url( __FILE__ ) . 'adhoc-style.css' );
}
add_action('wp_enqueue_scripts', 'custom_user_meta_enqueue_scripts');



// Enqueue the JavaScript file and pass API URL as a variable:
function income_planner_enqueue_scripts()
{
    wp_enqueue_script('external-api', plugin_dir_url(__FILE__) . 'income-planner.js', array('jquery'), '1.0', true);

    // Pass API URL as a variable to the income-planner.js file
    wp_localize_script(
        'external-api',
        'external_api_params',
        array(
            'api_url' => esc_js(admin_url('admin-ajax.php')),
            'security_json' => esc_js(wp_create_nonce('external-api-json-nonce')),
            'security_image' => esc_js(wp_create_nonce('external-api-image-nonce')),
            'security_pdf' => esc_js(wp_create_nonce('external-api-pdf-nonce')),
        )
    );

}
add_action('wp_enqueue_scripts', 'income_planner_enqueue_scripts');


function income_planner_form_header_shortcode($atts)
{
    ob_start(); // Start output buffering

    // Enqueue jQuery library
    wp_enqueue_script('jquery');


    // Display the buttons
    ?>

    <form id="income-planner-form">

        <div id="plannerMainFields">

            <label for="numberOfYears">Number of Years:<span class="saasify-required">*</span></label>
            <select id="numberOfYears" name="numberOfYears" required>
                <?php
                for ($i = 1; $i <= 35; $i++) {
                    $selected = ($i == 35) ? 'selected' : ''; // Check if current year is 35
                    echo "<option value='$i' $selected>$i</option>";
                }
                ?>
            </select><br>

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

            <label for="indexation">Indexation:<span class="saasify-required">*</span></label>
            <input type="text" id="indexation" name="indexation" required><br>

            <label for="retirementPot">Retirement Pot:<span class="saasify-required">*</span></label>
            <input type="text" id="retirementPot" name="retirementPot" required><br>

            <label for="investmentGrowth">Investment Growth:<span class="saasify-required">*</span></label>
            <input type="text" id="investmentGrowth" name="investmentGrowth" required><br>

        </div>

        <?php

        return ob_get_clean(); // Return the buffered output
}

function income_planner_client_1_input_shortcode($atts)
{
    ob_start(); // Start output buffering
    ?>
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

            <label for="client1StatePensionAmount">State Pension Amount:<span class="saasify-required">*</span></label>
            <input type="text" id="client1StatePensionAmount" name="client1StatePensionAmount" required><br>

            <label for="client1StatePensionAge">State Pension Age:<span class="saasify-required">*</span></label>
            <input type="text" id="client1StatePensionAge" name="client1StatePensionAge" required><br>

            <label for="client1OtherPensionAmount">Other Pension Amount:</label>
            <input type="text" id="client1OtherPensionAmount" name="client1OtherPensionAmount"><br>

            <label for="client1OtherPensionAge">Other Pension Age:</label>
            <input type="text" id="client1OtherPensionAge" name="client1OtherPensionAge"><br>

            <label for="client1OtherIncome">Other Income:</label>
            <input type="text" id="client1OtherIncome" name="client1OtherIncome"><br>

            <label for="client1RetirementIncomeLevel">Retirement Income Level:<span
                    class="saasify-required">*</span></label>
            <input type="text" id="client1RetirementIncomeLevel" name="client1RetirementIncomeLevel" required><br>


            <label for="client1ContributionsContainer">Adhoc Transactions:</label>
            <table id="client1ContributionsContainer">
                <tr>
                    <th><label for="client1AdhocTransactionAge">Age</label></th>
                    <th><label for="client1AdhocTransactionAmount">Amount</label></th>
                    <th class="remove-column"><label for="client1RemoveContribution">Delete?</label></th>
                </tr>

                <tr class="client1Contribution">
                    <td class="form-field">
                        <input type="text" name="client1AdhocTransactionAge[]" class="adhocTransactionAge">
                    </td>
                    <td class="form-field">
                        <input type="text" name="client1AdhocTransactionAmount[]" class="adhocTransactionAmount">
                    </td>
                    <td class="remove-button-column">
                        <!--<button type="button" class="client1RemoveContribution">Remove</button>-->
                    </td>
                </tr>
            </table>
        <button type="button" id="client1AddContribution">Add Contribution</button>
        </div>

        <?php

        return ob_get_clean(); // Return the buffered output
}

function income_planner_client_2_input_shortcode($atts)
{
    ob_start(); // Start output buffering
    ?>
        <div class="client-inputs" id="client2input" style="display: none;">

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

            <label for="client2StatePensionAmount">State Pension Amount:<span class="saasify-required">*</span></label>
            <input type="text" id="client2StatePensionAmount" name="client2StatePensionAmount"><br>

            <label for="client2StatePensionAge">State Pension Age:<span class="saasify-required">*</span></label>
            <input type="text" id="client2StatePensionAge" name="client2StatePensionAge"><br>

            <label for="client2OtherPensionAmount">Other Pension Amount:</label>
            <input type="text" id="client2OtherPensionAmount" name="client2OtherPensionAmount"><br>

            <label for="client2OtherPensionAge">Other Pension Age:</label>
            <input type="text" id="client2OtherPensionAge" name="client2OtherPensionAge"><br>

            <label for="client2OtherIncome">Other Income:</label>
            <input type="text" id="client2OtherIncome" name="client2OtherIncome"><br>

            <label for="client2RetirementIncomeLevel">Retirement Income Level:<span
                    class="saasify-required">*</span></label>
            <input type="text" id="client2RetirementIncomeLevel" name="client2RetirementIncomeLevel"><br>


            <label for="client2ContributionsContainer">Adhoc Transactions:</label>
            <table id="client2ContributionsContainer">
                <tr>
                    <th><label for="client2AdhocTransactionAge">Age</label></th>
                    <th><label for="client2AdhocTransactionAmount">Amount</label></th>
                    <th class="remove-column"><label for="client2RemoveContribution">Delete?</label></th>
                </tr>

                <tr class="client2Contribution">
                    <td class="form-field">
                        <input type="text" name="client2AdhocTransactionAge[]" class="adhocTransactionAge">
                    </td>
                    <td class="form-field">
                        <input type="text" name="client2AdhocTransactionAmount[]" class="adhocTransactionAmount">
                    </td>
                    <td class="remove-button-column">
                        <!--<button type="button" class="client2RemoveContribution">Remove</button>-->
                    </td>
                </tr>
            </table>

            <button type="button" id="client2AddContribution">Add Contribution</button>
        </div>


        <?php

        return ob_get_clean(); // Return the buffered output
}

function income_planner_form_footer_shortcode($atts)
{
    ob_start(); // Start output buffering
    ?>
        <div id="plannerSubmitButtons">
            <button type="submit" id="json-button">Show calculated data</button>
            <button type="submit" id="image-button">Preview Chart</button>
            <button type="submit" id="pdf-button">Download Report</button>
        </div>
    </form>

    <div id="plannerOutputArea">
        <div id="image-output"></div>
        <div id="json-output"></div>
    </div>

    <?php

    return ob_get_clean(); // Return the buffered output
}

// Code for dealing with chart colours saved as user meta data and setting / loading custom colours: 
function custom_user_meta_shortcode()
{

    ob_start(); // Start output buffering

    // Enqueue jQuery library
    wp_enqueue_script('jquery');

    $current_user_id = get_current_user_id();
    ?>

    <div class="custom-user-meta">
        <form id="custom-user-meta-form" action="<?php echo esc_url($_SERVER['REQUEST_URI']); ?>" method="post">
            <div style="display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center">
                <div>
                    <?php
                    echo get_meta_html($current_user_id, 'totalDrawdownColor', 'Total Drawdown Colour');
                    echo get_meta_html($current_user_id, 'statePensionPrimaryColor', 'Client 1 State Pension Colour');
                    echo get_meta_html($current_user_id, 'otherPensionPrimaryColor', 'Client 1 Other Pensions Colour');
                    echo get_meta_html($current_user_id, 'salaryPrimaryColor', 'Client 1 Salary Colour');
                    echo get_meta_html($current_user_id, 'otherIncomePrimaryColor', 'Client 1 Other Income Colour');
                    ?>
                </div>

                <div>
                    <?php
                    echo get_meta_html($current_user_id, 'totalFundValueColor', 'Total Fund Value Colour');
                    echo get_meta_html($current_user_id, 'statePensionSecondaryColor', 'Client 2 State Pension Colour');
                    echo get_meta_html($current_user_id, 'otherPensionSecondaryColor', 'Client 2 Other Pensions Colour');
                    echo get_meta_html($current_user_id, 'salarySecondaryColor', 'Client 2 Salary Colour');
                    echo get_meta_html($current_user_id, 'otherIncomeSecondaryColor', 'Client 2 Other Income Colour');
                    ?>
                </div>
            </div>

            <div
                style="margin: 1em auto 5px;display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; gap: 10px;">
                <input type="submit" name="save_custom_user_meta" value="Save Colours">
                <input type="button" name="cancel_custom_user_meta" value="Cancel">
                <input type="button" name="reset_custom_user_meta" value="Reset to Defaults">
            </div>
        </form>
    </div>

    <script>
        var chartColors = <?php echo json_encode(getChartColors()); ?>;
    </script>

    <?php
    return ob_get_clean(); // Return the buffered output
}

function get_meta_html($user_id, $chart_key, $description)
{
    $meta_value = get_meta_value($user_id, $chart_key);

    ob_start();
    ?>
    <div class="meta-field">
        <?php
        echo "<label for='$chart_key'>$description</label>";
        echo "<input type='text' class='color-picker' id='$chart_key' name='$chart_key' value='$meta_value'>";
        ?>
    </div>

    <?php
    return ob_get_clean();
}

function get_meta_value($user_id, $chartKey)
{
    $chartColors = array();

    // Map the $chartKey to the corresponding value key
    $value_key = mapChartKeyToValueKey($chartKey);

    $meta_value = get_user_meta($user_id, $value_key, true);
    $default_value = '#000000'; // Change this to your desired default value

    if ($meta_value !== false && !empty($meta_value) && preg_match('/^#[a-fA-F0-9]{6}$/', $meta_value)) {
        return $meta_value;
    } else {
        // Check if $chartColors is already populated
        if (empty($chartColors)) {
            $chartColors = getChartColors();
        }

        // Check if the $valueKey exists in $chartColors
        if (isset($chartColors[$chartKey])) {
            return $chartColors[$chartKey];
        } else {
            return $default_value;
        }
    }
}

// Register shortcode
add_shortcode('custom_user_meta', 'custom_user_meta_shortcode');

function mapChartKeyToValueKey($chartKey)
{
    // Map the $chartKey to the corresponding value key    
    return 'mepr_' . strtolower($chartKey);
}

function getChartColorsFromMeta()
{
    $user_id = get_current_user_id();

    $chartColors = array();

    // Retrieve the values from meta data using get_meta_value() function
    $chartColors['totalDrawdownColor'] = get_meta_value($user_id, 'totalDrawdownColor');
    $chartColors['statePensionPrimaryColor'] = get_meta_value($user_id, 'statePensionPrimaryColor');
    $chartColors['statePensionSecondaryColor'] = get_meta_value($user_id, 'statePensionSecondaryColor');
    $chartColors['otherPensionPrimaryColor'] = get_meta_value($user_id, 'otherPensionPrimaryColor');
    $chartColors['otherPensionSecondaryColor'] = get_meta_value($user_id, 'otherPensionSecondaryColor');
    $chartColors['salaryPrimaryColor'] = get_meta_value($user_id, 'salaryPrimaryColor');
    $chartColors['salarySecondaryColor'] = get_meta_value($user_id, 'salarySecondaryColor');
    $chartColors['otherIncomePrimaryColor'] = get_meta_value($user_id, 'otherIncomePrimaryColor');
    $chartColors['otherIncomeSecondaryColor'] = get_meta_value($user_id, 'otherIncomeSecondaryColor');
    $chartColors['totalFundValueColor'] = get_meta_value($user_id, 'totalFundValueColor');

    return $chartColors;
}




function getChartColors()
{
    // Make the API request using wp_remote_get()
    $response = wp_remote_get(API_URL . 'GetDefaultColorScheme');

    if (!is_wp_error($response) && $response['response']['code'] === 200) {
        $body = wp_remote_retrieve_body($response);

        // Attempt to parse the response as JSON
        $json_data = json_decode($body, true);

        if ($json_data !== null) {
            // If the response is valid JSON, return the data as an array
            return $json_data;
        }
    }

    // Fallback to the existing hard-coded array if the API request fails or the response is invalid
    return array(
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
    );
}

add_action('init', 'save_custom_user_meta');

function save_custom_user_meta()
{
    if (isset($_POST['save_custom_user_meta'])) {
        $current_user_id = get_current_user_id();

        // Update user meta data for each field
        update_user_meta($current_user_id, mapChartKeyToValueKey('totalDrawdownColor'), sanitize_text_field($_POST['totalDrawdownColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('statePensionPrimaryColor'), sanitize_text_field($_POST['statePensionPrimaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('statePensionSecondaryColor'), sanitize_text_field($_POST['statePensionSecondaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('otherPensionPrimaryColor'), sanitize_text_field($_POST['otherPensionPrimaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('otherPensionSecondaryColor'), sanitize_text_field($_POST['otherPensionSecondaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('salaryPrimaryColor'), sanitize_text_field($_POST['salaryPrimaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('salarySecondaryColor'), sanitize_text_field($_POST['salarySecondaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('otherIncomePrimaryColor'), sanitize_text_field($_POST['otherIncomePrimaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('otherIncomeSecondaryColor'), sanitize_text_field($_POST['otherIncomeSecondaryColor']));
        update_user_meta($current_user_id, mapChartKeyToValueKey('totalFundValueColor'), sanitize_text_field($_POST['totalFundValueColor']));
    }
}


//Rest of the Income Planner code:

// AJAX handler for the JSON data request
add_action('wp_ajax_external_api_json_request', 'handle_external_api_json_request');
add_action('wp_ajax_nopriv_external_api_json_request', 'handle_external_api_json_request');

// AJAX handler for the image request
add_action('wp_ajax_external_api_image_request', 'handle_external_api_image_request');
add_action('wp_ajax_nopriv_external_api_image_request', 'handle_external_api_image_request');

// AJAX handler for the PDF request
add_action('wp_ajax_external_api_pdf_request', 'handle_external_api_pdf_request');
add_action('wp_ajax_nopriv_external_api_pdf_request', 'handle_external_api_pdf_request');

function handle_external_api_request($api_endpoint, $data, $request_type)
{
    check_ajax_referer('external-api-' . $request_type . '-nonce', 'security');

    $api_url = API_URL . $api_endpoint;

    $response = wp_remote_post(
        $api_url,
        array(
            'headers' => array('Content-Type' => 'application/json'),
            'body' => json_encode($data),
            'timeout' => 30
        )
    );

    if (is_wp_error($response) || wp_remote_retrieve_response_code($response) !== 200) {
        $error_message = 'Failed to fetch data from API.';
        wp_send_json_error(array('message' => $error_message));
    } else {
        $body = wp_remote_retrieve_body($response);

        switch ($request_type) {
            case 'json':
                wp_send_json_success(array('json_data' => $body));
                break;
            case 'image':
                $encoded_image_data = base64_encode($body);
                wp_send_json_success(array('image_data' => $encoded_image_data));
                break;
            case 'pdf':
                $encoded_pdf_data = base64_encode($body);
                wp_send_json_success(array('pdf_data' => $encoded_pdf_data));
                break;
            default:
                $error_message = 'Invalid request type: ' . $request_type . '.';
                wp_send_json_error(array('message' => $error_message));
                break;
        }
    }

    wp_die();
}

function handle_external_api_json_request()
{
    $api_endpoint = 'RequestOutputData';
    $data = array(
        'pensionInputData' => $_POST['pensionInputData']
    );
    handle_external_api_request($api_endpoint, $data, 'json');
}

function handle_external_api_image_request()
{
    $api_endpoint = 'RequestChartImage';
    $data = array(
        'pensionInputData' => $_POST['pensionInputData'],
        'pensionChartColors' => getChartColorsFromMeta(),
        'imageSize' => array(
            'width' => 800,
            'height' => 600
        )
    );
    handle_external_api_request($api_endpoint, $data, 'image');
}

function handle_external_api_pdf_request()
{
    $api_endpoint = 'RequestReportPDF';
    $data = array(
        'pensionInputData' => $_POST['pensionInputData'],
        'pensionChartColors' => getChartColorsFromMeta()
    );
    handle_external_api_request($api_endpoint, $data, 'pdf');
}