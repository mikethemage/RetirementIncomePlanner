<?php
/**
 * Plugin Name: Custom User Meta Plugin
 * Description: Retrieves and displays user meta data - for testing purposes.
 * Version: 1.0.6
 * Author: Mike Dunn
 */

// Exit if accessed directly
if (!defined('ABSPATH')) {
    exit;
}

// Enqueue necessary scripts and styles
function custom_user_meta_enqueue_scripts()
{
    // Enqueue spectrum.js script
    wp_enqueue_script('spectrum-script', 'https://cdnjs.cloudflare.com/ajax/libs/spectrum/1.8.1/spectrum.min.js', array('jquery'), '1.8.1', true);

    // Enqueue spectrum.css stylesheet
    wp_enqueue_style('spectrum-style', 'https://cdnjs.cloudflare.com/ajax/libs/spectrum/1.8.1/spectrum.min.css');
}
add_action('wp_enqueue_scripts', 'custom_user_meta_enqueue_scripts');

// Plugin functionality
function custom_user_meta_shortcode()
{

    ob_start(); // Start output buffering

    // Enqueue jQuery library
    wp_enqueue_script('jquery');

    $current_user_id = get_current_user_id();
    ?>

    <div class="custom-user-meta">
        <form id="custom-user-meta-form" action="<?php echo esc_url($_SERVER['REQUEST_URI']); ?>" method="post">

            <?php
            echo get_meta_html($current_user_id, 'totalDrawdownColor', 'Total Drawdown Colour');
            echo get_meta_html($current_user_id, 'statePensionPrimaryColor', 'Client 1 State Pension Color');
            echo get_meta_html($current_user_id, 'statePensionSecondaryColor', 'Client 2 State Pension Color');
            echo get_meta_html($current_user_id, 'otherPensionPrimaryColor', 'Client 1 Other Pensions Color');
            echo get_meta_html($current_user_id, 'otherPensionSecondaryColor', 'Client 2 Other Pensions Color');
            echo get_meta_html($current_user_id, 'salaryPrimaryColor', 'Client 1 Salary Color');
            echo get_meta_html($current_user_id, 'salarySecondaryColor', 'Client 2 Salary Color');
            echo get_meta_html($current_user_id, 'otherIncomePrimaryColor', 'Client 1 Other Income Color');
            echo get_meta_html($current_user_id, 'otherIncomeSecondaryColor', 'Client 2 Other Income Color');
            echo get_meta_html($current_user_id, 'totalFundValueColor', 'Total Fund Value Colour');
            ?>

            <input type="submit" name="save_custom_user_meta" value="Save Colours">
            <input type="button" name="cancel_custom_user_meta" value="Cancel">
            <input type="button" name="reset_custom_user_meta" value="Reset to Defaults">



        </form>
    </div>

    <script>
        var chartColors = <?php echo json_encode(getChartColors()); ?>;

        jQuery(document).ready(function ($) {
            // Initialize the color picker
            $('.color-picker').spectrum({
                preferredFormat: 'hex',
                showInput: true,
                showPalette: true,
                palette: [
                    ['#000000', '#ffffff', '#ff0000', '#00ff00', '#0000ff'],
                    ['#ffff00', '#ff00ff', '#00ffff', '#ff9900', '#9900ff']
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
                    $(this).val(initialValue);
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
        });

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
    $api_url = 'http://localhost:5001/api/RetirementIncomePlanner/GetDefaultColorScheme';

    // Make the API request using wp_remote_get()
    $response = wp_remote_get($api_url);

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