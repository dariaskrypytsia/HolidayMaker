public class RumMeny(RumRepository rumRepo, BokningRepository bokningRepo)   // NYTT: bokningRepo
{
    public async Task SokOchBokaAsync(Kund? kund)                            // NYTT: nytt namn + kund
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

        // NYTT: från här börjar bokningen

        if (kund == null)
        {
            Console.WriteLine("Välj en kund först (meny 2) om du vill boka.");
            return;
        }

        Console.Write("Skriv numret på rummet du vill boka (Enter = avbryt): ");
        if (!int.TryParse(Console.ReadLine(), out int rumId))
        {
            Console.WriteLine("Ingen bokning gjordes.");
            return;
        }

        var valtRum = rum.Find(r => r.Id == rumId);

        if (valtRum == null)
        {
            Console.WriteLine("Det rummet finns inte i listan.");
            return;
        }

        if (!await bokningRepo.ArLedigtAsync(valtRum.Id, incheckning, utcheckning))
        {
            Console.WriteLine("Tyvärr, rummet är inte längre ledigt.");
            return;
        }

        long bokningsnummer = await bokningRepo.SkapaAsync(kund.Id, valtRum.Id, incheckning, utcheckning, antal);
        decimal totalpris = natter * valtRum.PrisPerNatt;

        Console.WriteLine($"Bokningen är klar! Bokningsnummer: {bokningsnummer}");
        Console.WriteLine($"{kund.Namn}, {valtRum.Boende} rum {valtRum.Rumsnummer}, {incheckning:yyyy-MM-dd} – {utcheckning:yyyy-MM-dd}");
        Console.WriteLine($"Totalpris: {natter} nätter × {valtRum.PrisPerNatt} kr = {totalpris} kr");
    }
}