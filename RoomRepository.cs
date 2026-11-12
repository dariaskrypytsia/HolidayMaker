using MySqlConnector;

public class RoomRepository(MySqlConnection connection)
{
    public async Task<List<AvailableRoom>> SearchAvailableAsync(string city, DateTime checkIn, DateTime checkOut, int guests)
    {
        var rooms = new List<AvailableRoom>();

        await using var command = new MySqlCommand(@"
            SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt
            FROM rum r
            JOIN boende bo ON bo.id = r.boende_id
            WHERE bo.ort = @city
              AND r.max_personer >= @guests
              AND NOT EXISTS (
                  SELECT 1
                  FROM bokning bk
                  WHERE bk.rum_id = r.id
                    AND bk.incheckning < @checkOut
                    AND bk.utcheckning > @checkIn
              )
            ORDER BY r.pris_per_natt;", connection);

        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@guests", guests);
        command.Parameters.AddWithValue("@checkIn", checkIn);
        command.Parameters.AddWithValue("@checkOut", checkOut);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            rooms.Add(new AvailableRoom(
                reader.GetInt32("id"),
                reader.GetString("boende"),
                reader.GetString("ort"),
                reader.GetString("rumsnummer"),
                reader.GetInt32("max_personer"),
                reader.GetDecimal("pris_per_natt")));
        }

        return rooms;
    }
}
