<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "cgd";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("connection failed: " . $conn->connect_error);
}
echo "connected successfully <br>";

$sql = "SELECT id, username, winrate FROM users";
$results = $conn->query($sql);

if ($results->num_rows > 0) {
    while($row = $results->fetch_assoc()) {
        echo "id: " . $row["id"]. " - Username: " . $row["username"]. " Win Rate " . $row["winrate"]. "<br>";
    }
}
else {
    echo "0 results";
}
$conn->close();

?>