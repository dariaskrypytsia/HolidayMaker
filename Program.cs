using MySqlConnector;

string connectionString =
    "Server=localhost;Port=3306;Database=holidaymaker;User ID=root;Password=kurslosenord;";

await using var connection = new MySqlConnection(connectionString);
await connection.OpenAsync();

Console.WriteLine("Ansluten till databasen!");

var customerRepo = new CustomerRepository(connection);
var customerMenu = new CustomerMenu(customerRepo);
var roomRepo = new RoomRepository(connection);
var bookingRepo = new BookingRepository(connection);
var roomMenu = new RoomMenu(roomRepo, bookingRepo);
var bookingMenu = new BookingMenu(bookingRepo);

bool running = true;
Customer? selectedCustomer = null;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Holiday Maker ===");
    Console.WriteLine($"Vald kund: {selectedCustomer?.Name ?? "ingen"}");
    Console.WriteLine("1. Registrera ny kund");
    Console.WriteLine("2. Välj kund");
    Console.WriteLine("3. Sök lediga rum och boka");
    Console.WriteLine("4. Visa kundens bokningar");
    Console.WriteLine("5. Ändra antal personer");
    Console.WriteLine("6. Avboka");
    Console.WriteLine("0. Avsluta");
    Console.Write("Välj: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await customerMenu.RegisterAsync();
            break;
        case "2":
            selectedCustomer = await customerMenu.SelectAsync();
            break;
        case "3":
            await roomMenu.SearchAndBookAsync(selectedCustomer);
            break;
        case "4":
        await bookingMenu.ShowForCustomerAsync(selectedCustomer);
            break;
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
