<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "cgd";

$usernameUpdateStats = $_POST["usernameupdatestats"];
$wonUpdateStats = $_POST["wonupdatestats"];
$silverUpdateStats = $_POST["silverupdatestats"];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("connection failed: " . $conn->connect_error);
}

if (strcmp($wonUpdateStats, "True") == 0) {
    $sqlUpdateWins = "UPDATE `users` SET `wins` = wins + 1 WHERE username = '" . $usernameUpdateStats . "'";
}
else {
    $sqlUpdateWins = "UPDATE `users` SET `losses` = losses + 1 WHERE username = '" . $usernameUpdateStats . "'";
}
$results = $conn->query($sqlUpdateWins);

$sqlUpdateSilver = "UPDATE `users` SET `silver` = silver + $silverUpdateStats WHERE username = '" . $usernameUpdateStats . "'";
$results = $conn->query($sqlUpdateSilver);

$conn->close();

?>