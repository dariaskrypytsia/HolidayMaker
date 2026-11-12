using MySqlConnector;

public class BokningRepository(MySqlConnection connection)
{
    public async Task<bool> ArLedigtAsync(int rumId, DateTime incheckning, DateTime utcheckning)
    {
        await using var command = new MySqlCommand(@"
            SELECT COUNT(*)
            FROM bokning
            WHERE rum_id = @rumId
              AND incheckning < @utcheckning
              AND utcheckning > @incheckning;", connection);

        command.Parameters.AddWithValue("@rumId", rumId);
        command.Parameters.AddWithValue("@incheckning", incheckning);
        command.Parameters.AddWithValue("@utcheckning", utcheckning);

        int antal = Convert.ToInt32(await command.ExecuteScalarAsync());
        return antal == 0;
    }

    public async Task<long> SkapaAsync(int kundId, int rumId, DateTime incheckning, DateTime utcheckning, int antalPersoner)
    {
        await using var command = new MySqlCommand(@"
            INSERT INTO bokning (kund_id, rum_id, incheckning, utcheckning, antal_personer)
            VALUES (@kundId, @rumId, @incheckning, @utcheckning, @antal);", connection);

        command.Parameters.AddWithValue("@kundId", kundId);
        command.Parameters.AddWithValue("@rumId", rumId);
        command.Parameters.AddWithValue("@incheckning", incheckning);
        command.Parameters.AddWithValue("@utcheckning", utcheckning);
        command.Parameters.AddWithValue("@antal", antalPersoner);

        await command.ExecuteNonQueryAsync();
        return command.LastInsertedId;
    }
}