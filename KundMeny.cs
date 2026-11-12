using MySqlConnector;

public class KundMeny(KundRepository kundRepo)
{
    public async Task RegistreraAsync()
    {
        Console.Write("Namn: ");
        string namn = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Telefon: ");
        string telefon = Console.ReadLine()?.Trim() ?? "";

        Console.Write("E-post (tryck Enter för att hoppa över): ");
        string epost = Console.ReadLine()?.Trim() ?? "";

        if (namn == "" || telefon == "")
        {
            Console.WriteLine("Namn och telefon måste fyllas i.");
            return;
        }

        try
        {
            long id = await kundRepo.RegistreraAsync(namn, telefon, epost == "" ? null : epost);
            Console.WriteLine($"Kunden är registrerad med kundnummer {id}.");
        }
        catch (MySqlException)
        {
            Console.WriteLine("Kunde inte spara kunden. Kanske finns e-posten redan?");
        }
    }
}