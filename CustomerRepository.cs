using MySqlConnector;

public class CustomerRepository(MySqlConnection connection)
{
    public async Task<long> RegisterAsync(string name, string phone, string? email)
    {
        await using var command = new MySqlCommand(
            "INSERT INTO kund (namn, telefon, epost) VALUES (@name, @phone, @email);",
            connection);

        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@phone", phone);
        command.Parameters.AddWithValue("@email", email);

        await command.ExecuteNonQueryAsync();
        return command.LastInsertedId;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var customers = new List<Customer>();

        await using var command = new MySqlCommand(
            "SELECT id, namn, telefon, epost FROM kund ORDER BY namn;",
            connection);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            customers.Add(new Customer(
                reader.GetInt32("id"),
                reader.GetString("namn"),
                reader.GetString("telefon"),
                reader.IsDBNull(reader.GetOrdinal("epost")) ? null : reader.GetString("epost")));
        }

        return customers;
    }
}
