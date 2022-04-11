<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "cgd";

$usernameGetStats = $_POST["usernamegetstats"];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("connection failed: " . $conn->connect_error);
}

$sqlGetStats = "SELECT silver, wins, losses FROM users WHERE username = '" . $usernameGetStats . "'";
//$sql = "INSERT INTO users (username, password, silver, wins, losses) VALUES ('" . $registerusername . "'");
$results = $conn->query($sqlGetStats);

if ($results->num_rows > 0) {
    // Username is already taken
    $rows = array();
    while($row = $results->fetch_assoc()) {
        $rows[] = $row;
    }

    echo json_encode($rows);
}
else {
    echo "error";
}
$conn->close();

?>