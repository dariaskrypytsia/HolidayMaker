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

    public async Task<Kund?> ValjAsync()
    {
        var kunder = await kundRepo.HamtaAllaAsync();

        foreach (var kund in kunder)
        {
            Console.WriteLine($"{kund.Id}. {kund.Namn} ({kund.Telefon})");
        }

        Console.Write("Skriv kundnummer: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Du måste skriva ett nummer.");
            return null;
        }

        var vald = kunder.Find(k => k.Id == id);

        if (vald == null)
        {
            Console.WriteLine("Det finns ingen kund med det numret.");
        }

        return vald;
    }
}
