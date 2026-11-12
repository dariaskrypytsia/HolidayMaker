using MySqlConnector;

public class RumRepository(MySqlConnection connection)
{
    public async Task<List<LedigtRum>> SokLedigaAsync(string ort, DateTime incheckning, DateTime utcheckning, int antalPersoner)
    {
        var rum = new List<LedigtRum>();

        await using var command = new MySqlCommand(@"
            SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt
            FROM rum r
            JOIN boende bo ON bo.id = r.boende_id
            WHERE bo.ort = @ort
              AND r.max_personer >= @antal
              AND NOT EXISTS (
                  SELECT 1
                  FROM bokning bk
                  WHERE bk.rum_id = r.id
                    AND bk.incheckning < @utcheckning
                    AND bk.utcheckning > @incheckning
              )
            ORDER BY r.pris_per_natt;", connection);

        command.Parameters.AddWithValue("@ort", ort);
        command.Parameters.AddWithValue("@antal", antalPersoner);
        command.Parameters.AddWithValue("@incheckning", incheckning);
        command.Parameters.AddWithValue("@utcheckning", utcheckning);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            rum.Add(new LedigtRum(
                reader.GetInt32("id"),
                reader.GetString("boende"),
                reader.GetString("ort"),
                reader.GetString("rumsnummer"),
                reader.GetInt32("max_personer"),
                reader.GetDecimal("pris_per_natt")));
        }

        return rum;
    }
}