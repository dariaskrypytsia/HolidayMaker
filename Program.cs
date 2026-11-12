using MySqlConnector;

string connectionString =
    "Server=localhost;Port=3306;Database=holidaymaker;User ID=root;Password=kurslosenord;";

await using var connection = new MySqlConnection(connectionString);
await connection.OpenAsync();

Console.WriteLine("Ansluten till databasen!");