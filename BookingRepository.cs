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
}
