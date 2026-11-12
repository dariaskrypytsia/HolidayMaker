public class RoomMenu(RoomRepository roomRepo, BookingRepository bookingRepo)
{
    private static bool AskYesNo(string question)
    {
        Console.Write($"{question} (j/n): ");
        string answer = Console.ReadLine()?.Trim().ToLower() ?? "";
        return answer == "j";
    }

    private static int? AskMaxDistance(string question)
    {
        while (true)
        {
            Console.Write($"{question} (meter, Enter = spelar ingen roll): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (input == "")
            {
                return null;
            }

            if (int.TryParse(input, out int meters) && meters >= 0)
            {
                return meters;
            }

            Console.WriteLine("Skriv ett heltal, eller tryck Enter.");
        }
    }

    public async Task SearchAndBookAsync(Customer? customer)
    {
        Console.Write("Ort: ");
        string city = Console.ReadLine()?.Trim() ?? "";

        if (city == "")
        {
            Console.WriteLine("Du måste skriva en ort.");
            return;
        }

        Console.Write("Incheckning (ÅÅÅÅ-MM-DD): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkIn))
        {
            Console.WriteLine("Fel datum. Skriv till exempel 2026-12-10.");
            return;
        }

        Console.Write("Utcheckning (ÅÅÅÅ-MM-DD): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOut))
        {
            Console.WriteLine("Fel datum. Skriv till exempel 2026-12-13.");
            return;
        }

        if (checkOut <= checkIn)
        {
            Console.WriteLine("Utcheckning måste vara efter incheckning.");
            return;
        }

        Console.Write("Antal personer: ");
        if (!int.TryParse(Console.ReadLine(), out int guests) || guests < 1)
        {
            Console.WriteLine("Antal personer måste vara ett heltal, minst 1.");
            return;
        }

        bool needPool = AskYesNo("Måste det finnas pool?");
        bool needRestaurant = AskYesNo("Måste det finnas restaurang?");
        int? maxBeach = AskMaxDistance("Max avstånd till strand");
        int? maxCenter = AskMaxDistance("Max avstånd till centrum");

        Console.Write("Sortera efter: 1 = pris (lägst först), 2 = omdöme (högst först): ");
        bool sortByRating = Console.ReadLine()?.Trim() == "2";

        var filter = new SearchFilter(needPool, needRestaurant, maxBeach, maxCenter, sortByRating);
        var rooms = await roomRepo.SearchAvailableAsync(city, checkIn, checkOut, guests, filter);

        if (rooms.Count == 0)
        {
            Console.WriteLine("Inga lediga rum hittades.");
            return;
        }

        int nights = (checkOut - checkIn).Days;
        Console.WriteLine($"Lediga rum ({nights} nätter):");

        foreach (var room in rooms)
        {
            Console.WriteLine($"{room.Id}. {room.Accommodation}, {room.City} – rum {room.RoomNumber}, " +
                $"max {room.MaxGuests} pers, {room.PricePerNight} kr/natt, omdöme {room.Rating}/5");
        }

        if (customer == null)
        {
            Console.WriteLine("Välj en kund först (meny 2) om du vill boka.");
            return;
        }

        Console.Write("Skriv numret på rummet du vill boka (Enter = avbryt): ");
        if (!int.TryParse(Console.ReadLine(), out int roomId))
        {
            Console.WriteLine("Ingen bokning gjordes.");
            return;
        }

        var selectedRoom = rooms.Find(r => r.Id == roomId);

        if (selectedRoom == null)
        {
            Console.WriteLine("Det rummet finns inte i listan.");
            return;
        }

        if (!await bookingRepo.IsAvailableAsync(selectedRoom.Id, checkIn, checkOut))
        {
            Console.WriteLine("Tyvärr, rummet är inte längre ledigt.");
            return;
        }

        long bookingId = await bookingRepo.CreateAsync(customer.Id, selectedRoom.Id, checkIn, checkOut, guests);
        decimal totalPrice = nights * selectedRoom.PricePerNight;

        Console.WriteLine($"Bokningen är klar! Bokningsnummer: {bookingId}");
        Console.WriteLine($"{customer.Name}, {selectedRoom.Accommodation} rum {selectedRoom.RoomNumber}, {checkIn:yyyy-MM-dd} – {checkOut:yyyy-MM-dd}");
        Console.WriteLine($"Totalpris: {nights} nätter × {selectedRoom.PricePerNight} kr = {totalPrice} kr");
    }
}