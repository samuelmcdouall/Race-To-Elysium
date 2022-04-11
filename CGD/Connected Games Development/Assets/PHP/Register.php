<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "cgd";

$registerusername = $_POST["registerusername"];
$registerpassword = $_POST["registerpassword"];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("connection failed: " . $conn->connect_error);
}
echo "connected successfully <br>";

$sqlLoginCheck = "SELECT password FROM users WHERE username = '" . $registerusername . "'";
//$sql = "INSERT INTO users (username, password, silver, wins, losses) VALUES ('" . $registerusername . "'");
$results = $conn->query($sqlLoginCheck);

if ($results->num_rows > 0) {
    // Username is already taken
    echo "Username already taken (UAT)";
}
else {
    echo "Create user";
    $sqlRegister = "INSERT INTO users (username, password) VALUES ('" . $registerusername . "', '" . $registerpassword . "')";
    if ($conn->query($sqlRegister) === TRUE) {
        echo "New user created successfully! (NUCS)";
    }
    else {
        echo "Error: " . $sqlRegister . "<br>" . $conn->error;
    }
}
$conn->close();

?>