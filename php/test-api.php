function test_api_request() {
    $url = 'https://testapi20230512145105.azurewebsites.net/api/Addition';
    $response = wp_remote_request( $url );
    if ( is_wp_error( $response ) ) {
        $error_message = $response->get_error_message();
        return "Something went wrong: $error_message";
    } else {
        $body = wp_remote_retrieve_body( $response );
        return "Response: $body";
    }
}
