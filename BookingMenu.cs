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
}