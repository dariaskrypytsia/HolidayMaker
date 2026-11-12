public class BookingMenu(BookingRepository bookingRepo)
{
    public async Task<List<BookingInfo>> ShowForCustomerAsync(Customer? customer)
    {
        if (customer == null)
        {
            Console.WriteLine("Välj en kund först (meny 2).");
            return new List<BookingInfo>();
        }

        var bookings = await bookingRepo.GetForCustomerAsync(customer.Id);

        if (bookings.Count == 0)
        {
            Console.WriteLine($"{customer.Name} har inga bokningar.");
            return bookings;
        }

        Console.WriteLine($"Bokningar för {customer.Name}:");

        foreach (var booking in bookings)
        {
            Console.WriteLine($"Nr {booking.Id}: {booking.Accommodation}, rum {booking.RoomNumber}, " +
                $"{booking.CheckIn:yyyy-MM-dd} – {booking.CheckOut:yyyy-MM-dd}, " +
                $"{booking.Guests} pers, {booking.TotalPrice} kr");
        }

        return bookings;
    }

    public async Task CancelAsync(Customer? customer)
    {
        var bookings = await ShowForCustomerAsync(customer);

        if (customer == null || bookings.Count == 0)
        {
            return;
        }

        Console.Write("Skriv bokningsnummer som ska avbokas (Enter = avbryt): ");
        if (!int.TryParse(Console.ReadLine(), out int bookingId))
        {
            Console.WriteLine("Ingen bokning avbokades.");
            return;
        }

        var selectedBooking = bookings.Find(b => b.Id == bookingId);

        if (selectedBooking == null)
        {
            Console.WriteLine("Det bokningsnumret finns inte i listan.");
            return;
        }

        Console.Write($"Vill du verkligen avboka nr {selectedBooking.Id}? (j/n): ");
        string answer = Console.ReadLine()?.Trim().ToLower() ?? "";

        if (answer != "j")
        {
            Console.WriteLine("Ingen bokning avbokades.");
            return;
        }

        int deleted = await bookingRepo.DeleteAsync(selectedBooking.Id, customer.Id);

        if (deleted == 1)
        {
            Console.WriteLine($"Bokning nr {selectedBooking.Id} är avbokad. Rummet är ledigt igen.");
        }
        else
        {
            Console.WriteLine("Bokningen kunde inte hittas.");
        }
    }
        public async Task ChangeGuestsAsync(Customer? customer)
    {
        var bookings = await ShowForCustomerAsync(customer);

        if (customer == null || bookings.Count == 0)
        {
            return;
        }

        Console.Write("Skriv bokningsnummer som ska ändras (Enter = avbryt): ");
        if (!int.TryParse(Console.ReadLine(), out int bookingId))
        {
            Console.WriteLine("Ingen ändring gjordes.");
            return;
        }

        var selectedBooking = bookings.Find(b => b.Id == bookingId);

        if (selectedBooking == null)
        {
            Console.WriteLine("Det bokningsnumret finns inte i listan.");
            return;
        }

        Console.Write($"Nytt antal personer (nu {selectedBooking.Guests}): ");
        if (!int.TryParse(Console.ReadLine(), out int guests) || guests < 1)
        {
            Console.WriteLine("Antal personer måste vara ett heltal, minst 1.");
            return;
        }

        int updated = await bookingRepo.UpdateGuestsAsync(selectedBooking.Id, customer.Id, guests);

        if (updated == 1)
        {
            Console.WriteLine($"Bokning nr {selectedBooking.Id} gäller nu {guests} personer.");
        }
        else
        {
            Console.WriteLine("Det får inte plats så många personer i rummet.");
        }
    }
}