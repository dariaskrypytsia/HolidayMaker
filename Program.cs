using MySqlConnector;

string connectionString =
    "Server=localhost;Port=3306;Database=holidaymaker;User ID=root;Password=kurslosenord;";

await using var connection = new MySqlConnection(connectionString);
await connection.OpenAsync();

Console.WriteLine("Ansluten till databasen!");

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Holiday Maker ===");
    Console.WriteLine("1. Registrera ny kund");
    Console.WriteLine("2. Välj kund");
    Console.WriteLine("3. Sök lediga rum och boka");
    Console.WriteLine("4. Visa kundens bokningar");
    Console.WriteLine("5. Ändra antal personer");
    Console.WriteLine("6. Avboka");
    Console.WriteLine("0. Avsluta");
    Console.Write("Välj: ");

    string? val = Console.ReadLine();

    switch (val)
    {
        case "1":
        case "2":
        case "3":
        case "4":
        case "5":
        case "6":
            Console.WriteLine("Kommer snart.");
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Fel val, försök igen.");
            break;
    }
}

Console.WriteLine("Hej då!");