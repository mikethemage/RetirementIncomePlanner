<?php 

/*
Plugin Name: Test API Plugin
Description: Wordpress plugin to query test api
*/

defined( 'ABSPATH') || die('Unauthorized access');

add_shortcode('external_data','test_api_request');

function test_api_request() {
    $endpoint = 'http://localhost:5000/api/Addition';

    $requestbody = [
        'operand1' => 2,
        'operand2' => 2,
    ];

    $requestbody = wp_json_encode( $requestbody );

    $options = [
        'body'        => $requestbody,
        'headers'     => [
            'Content-Type' => 'application/json',
        ],
        'timeout'     => 60,
        'redirection' => 5,
        'blocking'    => true,
        'httpversion' => '1.0',
        'sslverify'   => false,
        'data_format' => 'body',
    ];

    $response = wp_remote_post( $endpoint, $options );
    if ( is_wp_error( $response ) ) {
        $error_message = $response->get_error_message();
        return "Something went wrong: $error_message";
    } else {
        $responsebody = wp_remote_retrieve_body( $response );
        $responsebody = json_decode($responsebody, true);
        return "Response: ".$responsebody["result"];
    }
}