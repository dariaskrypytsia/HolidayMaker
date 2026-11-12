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
}