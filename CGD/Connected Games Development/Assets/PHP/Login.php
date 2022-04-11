<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "cgd";

$loginusername = $_POST["loginusername"];
$loginpassword = $_POST["loginpassword"];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("connection failed: " . $conn->connect_error);
}
echo "connected successfully <br>";

$sql = "SELECT password FROM users WHERE username = '" . $loginusername . "'";
$results = $conn->query($sql);

if ($results->num_rows > 0) {
    while($row = $results->fetch_assoc()) {
        if ($row["password"] == $loginpassword) {
            echo "login success (LS)";

        }
        else {
            echo "wrong credentials (WC)";
        }
    }
}
else {
    echo "username does not exist (UDNE)";
}
$conn->close();

?>