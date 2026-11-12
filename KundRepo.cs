using MySqlConnector;

public class KundRepository(MySqlConnection connection)
{
    public async Task<long> RegistreraAsync(string namn, string telefon, string? epost)
    {
        await using var command = new MySqlCommand(
            "INSERT INTO kund (namn, telefon, epost) VALUES (@namn, @telefon, @epost);",
            connection);

        command.Parameters.AddWithValue("@namn", namn);
        command.Parameters.AddWithValue("@telefon", telefon);
        command.Parameters.AddWithValue("@epost", epost);

        await command.ExecuteNonQueryAsync();
        return command.LastInsertedId;
    }

    public async Task<List<Kund>> HamtaAllaAsync()
    {
        var kunder = new List<Kund>();

        await using var command = new MySqlCommand(
            "SELECT id, namn, telefon, epost FROM kund ORDER BY namn;",
            connection);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            kunder.Add(new Kund(
                reader.GetInt32("id"),
                reader.GetString("namn"),
                reader.GetString("telefon"),
                reader.IsDBNull(reader.GetOrdinal("epost")) ? null : reader.GetString("epost")));
        }

        return kunder;
    }
}