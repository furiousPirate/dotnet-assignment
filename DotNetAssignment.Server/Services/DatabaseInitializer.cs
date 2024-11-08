using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace SalesmanCommisionReport.Server.Services
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task InitializeAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Create tables
                await CreateTablesAsync(connection);
            }
        }

        private async Task CreateTablesAsync(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = ProductDetailsTable;
                await command.ExecuteNonQueryAsync();
            }
        }

        const string ProductDetailsTable = @"
        CREATE TABLE IF NOT EXISTS ProductDetails (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Image TEXT NOT NULL,
            Title TEXT NOT NULL,
            Description TEXT NOT NULL,
            Quantity INTEGER NOT NULL,
            Price REAL NOT NULL,
            Date DATETIME NOT NULL
        );";
    }
}
