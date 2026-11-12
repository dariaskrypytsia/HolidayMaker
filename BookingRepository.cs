using MySqlConnector;

public class BookingRepository(MySqlConnection connection)
{
    public async Task<bool> IsAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        await using var command = new MySqlCommand(@"
            SELECT COUNT(*)
            FROM bokning
            WHERE rum_id = @roomId
              AND incheckning < @checkOut
              AND utcheckning > @checkIn;", connection);

        command.Parameters.AddWithValue("@roomId", roomId);
        command.Parameters.AddWithValue("@checkIn", checkIn);
        command.Parameters.AddWithValue("@checkOut", checkOut);

        int count = Convert.ToInt32(await command.ExecuteScalarAsync());
        return count == 0;
    }

    public async Task<long> CreateAsync(int customerId, int roomId, DateTime checkIn, DateTime checkOut, int guests)
    {
        await using var command = new MySqlCommand(@"
            INSERT INTO bokning (kund_id, rum_id, incheckning, utcheckning, antal_personer)
            VALUES (@customerId, @roomId, @checkIn, @checkOut, @guests);", connection);

        command.Parameters.AddWithValue("@customerId", customerId);
        command.Parameters.AddWithValue("@roomId", roomId);
        command.Parameters.AddWithValue("@checkIn", checkIn);
        command.Parameters.AddWithValue("@checkOut", checkOut);
        command.Parameters.AddWithValue("@guests", guests);

        await command.ExecuteNonQueryAsync();
        return command.LastInsertedId;
    }

    public async Task<List<BookingInfo>> GetForCustomerAsync(int customerId)
    {
        var bookings = new List<BookingInfo>();

        await using var command = new MySqlCommand(@"
            SELECT bk.id, bo.namn AS boende, r.rumsnummer, bk.incheckning, bk.utcheckning, bk.antal_personer,
                   DATEDIFF(bk.utcheckning, bk.incheckning) * r.pris_per_natt AS totalpris
            FROM bokning bk
            JOIN rum r ON r.id = bk.rum_id
            JOIN boende bo ON bo.id = r.boende_id
            WHERE bk.kund_id = @customerId
            ORDER BY bk.incheckning;", connection);

        command.Parameters.AddWithValue("@customerId", customerId);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            bookings.Add(new BookingInfo(
                reader.GetInt32("id"),
                reader.GetString("boende"),
                reader.GetString("rumsnummer"),
                reader.GetDateTime("incheckning"),
                reader.GetDateTime("utcheckning"),
                reader.GetInt32("antal_personer"),
                reader.GetDecimal("totalpris")));
        }

        return bookings;
    }
        public async Task<int> DeleteAsync(int bookingId, int customerId)
    {
        await using var command = new MySqlCommand(
            "DELETE FROM bokning WHERE id = @bookingId AND kund_id = @customerId;",
            connection);

        command.Parameters.AddWithValue("@bookingId", bookingId);
        command.Parameters.AddWithValue("@customerId", customerId);

        return await command.ExecuteNonQueryAsync();
    }
}
