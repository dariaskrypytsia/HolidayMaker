using MySqlConnector;

public class RoomRepository(MySqlConnection connection)
{
    public async Task<List<AvailableRoom>> SearchAvailableAsync(string city, DateTime checkIn, DateTime checkOut, int guests, SearchFilter filter)
    {
        var rooms = new List<AvailableRoom>();

        string orderBy = filter.SortByRating
            ? "bo.omdome DESC, r.pris_per_natt"
            : "r.pris_per_natt";

        await using var command = new MySqlCommand($@"
            SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt, bo.omdome
            FROM rum r
            JOIN boende bo ON bo.id = r.boende_id
            WHERE bo.ort = @city
              AND r.max_personer >= @guests
              AND (@needPool = 0 OR bo.pool = 1)
              AND (@needRestaurant = 0 OR bo.restaurang = 1)
              AND (@maxBeach IS NULL OR bo.avstand_strand <= @maxBeach)
              AND (@maxCenter IS NULL OR bo.avstand_centrum <= @maxCenter)
              AND NOT EXISTS (
                  SELECT 1
                  FROM bokning bk
                  WHERE bk.rum_id = r.id
                    AND bk.incheckning < @checkOut
                    AND bk.utcheckning > @checkIn
              )
            ORDER BY {orderBy};", connection);

        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@guests", guests);
        command.Parameters.AddWithValue("@checkIn", checkIn);
        command.Parameters.AddWithValue("@checkOut", checkOut);
        command.Parameters.AddWithValue("@needPool", filter.NeedPool);
        command.Parameters.AddWithValue("@needRestaurant", filter.NeedRestaurant);
        command.Parameters.AddWithValue("@maxBeach", filter.MaxBeach);
        command.Parameters.AddWithValue("@maxCenter", filter.MaxCenter);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            rooms.Add(new AvailableRoom(
                reader.GetInt32("id"),
                reader.GetString("boende"),
                reader.GetString("ort"),
                reader.GetString("rumsnummer"),
                reader.GetInt32("max_personer"),
                reader.GetDecimal("pris_per_natt"),
                reader.GetInt32("omdome")));
        }

        return rooms;
    }
}