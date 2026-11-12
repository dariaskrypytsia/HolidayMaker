using MySqlConnector;

public class CustomerMenu(CustomerRepository customerRepo)
{
    public async Task RegisterAsync()
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Telefon: ");
        string phone = Console.ReadLine()?.Trim() ?? "";

        Console.Write("E-post (tryck Enter för att hoppa över): ");
        string email = Console.ReadLine()?.Trim() ?? "";

        if (name == "" || phone == "")
        {
            Console.WriteLine("Namn och telefon måste fyllas i.");
            return;
        }

        try
        {
            long id = await customerRepo.RegisterAsync(name, phone, email == "" ? null : email);
            Console.WriteLine($"Kunden är registrerad med kundnummer {id}.");
        }
        catch (MySqlException)
        {
            Console.WriteLine("Kunde inte spara kunden. Kanske finns e-posten redan?");
        }
    }

    public async Task<Customer?> SelectAsync()
    {
        var customers = await customerRepo.GetAllAsync();

        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.Id}. {customer.Name} ({customer.Phone})");
        }

        Console.Write("Skriv kundnummer: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Du måste skriva ett nummer.");
            return null;
        }

        var selected = customers.Find(c => c.Id == id);

        if (selected == null)
        {
            Console.WriteLine("Det finns ingen kund med det numret.");
        }

        return selected;
    }
}
