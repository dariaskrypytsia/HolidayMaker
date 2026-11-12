public class RumMeny(RumRepository rumRepo)
{
    public async Task SokAsync()
    {
        Console.Write("Ort: ");
        string ort = Console.ReadLine()?.Trim() ?? "";

        if (ort == "")
        {
            Console.WriteLine("Du måste skriva en ort.");
            return;
        }

        Console.Write("Incheckning (ÅÅÅÅ-MM-DD): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime incheckning))
        {
            Console.WriteLine("Fel datum. Skriv till exempel 2026-12-10.");
            return;
        }

        Console.Write("Utcheckning (ÅÅÅÅ-MM-DD): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime utcheckning))
        {
            Console.WriteLine("Fel datum. Skriv till exempel 2026-12-13.");
            return;
        }

        if (utcheckning <= incheckning)
        {
            Console.WriteLine("Utcheckning måste vara efter incheckning.");
            return;
        }

        Console.Write("Antal personer: ");
        if (!int.TryParse(Console.ReadLine(), out int antal) || antal < 1)
        {
            Console.WriteLine("Antal personer måste vara ett heltal, minst 1.");
            return;
        }

        var rum = await rumRepo.SokLedigaAsync(ort, incheckning, utcheckning, antal);

        if (rum.Count == 0)
        {
            Console.WriteLine("Inga lediga rum hittades.");
            return;
        }

        int natter = (utcheckning - incheckning).Days;
        Console.WriteLine($"Lediga rum ({natter} nätter):");

        foreach (var r in rum)
        {
            Console.WriteLine($"{r.Id}. {r.Boende}, {r.Ort} – rum {r.Rumsnummer}, max {r.MaxPersoner} pers, {r.PrisPerNatt} kr/natt");
        }
    }
}