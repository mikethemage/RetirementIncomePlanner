<?php
/*
Plugin Name: API Form Plugin
*/

// Enqueue the JavaScript file
function api_form_enqueue_scripts() {
    wp_enqueue_script( 'api-form-script', plugin_dir_url( __FILE__ ) . 'api-form.js', array( 'jquery' ), '1.0', true );
}
add_action( 'wp_enqueue_scripts', 'api_form_enqueue_scripts' );

// Handle the API request
function api_form_handle_request() {
    // Check if the request is a POST request and has the necessary data
    if ( $_SERVER['REQUEST_METHOD'] === 'POST' && isset( $_POST['clientCount'], $_POST['firstName1'], $_POST['lastName1'] ) ) {
        // Retrieve the data from the request
        $clientCount = intval( $_POST['clientCount'] );
        $firstName1 = sanitize_text_field( $_POST['firstName1'] );
        $lastName1 = sanitize_text_field( $_POST['lastName1'] );
        $firstName2 = '';
        $lastName2 = '';

        // If two clients are selected, retrieve the values of the second client
        if ( $clientCount === 2 && isset( $_POST['firstName2'], $_POST['lastName2'] ) ) {
            $firstName2 = sanitize_text_field( $_POST['firstName2'] );
            $lastName2 = sanitize_text_field( $_POST['lastName2'] );
        }

        // Process the data and retrieve the full names
        $fullName1 = $firstName1 . ' ' . $lastName1;
        $fullName2 = ( $clientCount === 2 ) ? $firstName2 . ' ' . $lastName2 : '';

        // Prepare the response data
        $response = array(
            'fullName1' => $fullName1,
            'fullName2' => $fullName2
        );

        // Send the response as JSON
        header( 'Content-Type: application/json' );
        echo json_encode( $response );
        exit;
    }
}
add_action( 'wp_ajax_api_form_submit', 'api_form_handle_request' );
add_action( 'wp_ajax_nopriv_api_form_submit', 'api_form_handle_request' );

// Shortcode for displaying the form
function api_form_shortcode() {
    ob_start();
    ?>
    <form id="api-form">
        <div class="client-toggle-wrap">
            <label for="client-toggle">Number of Clients:</label>
            <div class="toggle-container">
                <span class="toggle-label">One Client</span>
                <label class="switch">
                    <input type="checkbox" id="client-toggle" name="client-toggle" checked>
                    <span class="slider"></span>
                </label>
                <span class="toggle-label">Two Clients</span>
            </div>
        </div>

        <div id="client-info">
            <div class="client-inputs">
                <label for="first-name-1">First Name (Client 1):</label>
                <input type="text" id="first-name-1" name="first-name-1">

                <label for="last-name-1">Last Name (Client 1):</label>
                <input type="text" id="last-name-1" name="last-name-1">
            </div>

            <div class="client-inputs" style="display: none;">
                <label for="first-name-2">First Name (Client 2):</label>
                <input type="text" id="first-name-2" name="first-name-2">

                <label for="last-name-2">Last Name (Client 2):</label>
                <input type="text" id="last-name-2" name="last-name-2">
            </div>
        </div>

        <button type="button" onclick="submitForm()">Submit</button>
    </form>

    <script>
        function submitForm() {
            var clientToggle = document.getElementById("client-toggle");
            var firstName1 = document.getElementById("first-name-1").value;
            var lastName1 = document.getElementById("last-name-1").value;
            var firstName2 = document.getElementById("first-name-2").value;
            var lastName2 = document.getElementById("last-name-2").value;

            var clientCount = clientToggle.checked ? 2 : 1;

            var requestData = {
                clientCount: clientCount,
                firstName1: firstName1,
                lastName1: lastName1,
                firstName2: firstName2,
                lastName2: lastName2
            };

            fetch("<?php echo admin_url('admin-ajax.php'); ?>?action=api_form_submit", {
                method: "POST",
                body: JSON.stringify(requestData)
            })
            .then(response => response.json())
            .then(data => {
                console.log(data);
            })
            .catch(error => {
                console.error("Error:", error);
            });
        }

        var clientToggle = document.getElementById("client-toggle");
        var clientInfo = document.getElementById("client-info");

        clientToggle.addEventListener("change", function() {
            var clientInputs2 = document.querySelector(".client-inputs:nth-child(2)");

            if (clientToggle.checked) {
                clientInputs2.style.display = "none";
            } else {
                clientInputs2.style.display = "block";
            }
        });
    </script>
    <?php
    return ob_get_clean();
}
add_shortcode( 'api_form', 'api_form_shortcode' );
