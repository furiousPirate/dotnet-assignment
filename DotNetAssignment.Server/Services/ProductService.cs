using DotNetAssignment.Server.Models;
using Microsoft.Data.Sqlite;

namespace DotNetAssignment.Server.Services
{
    public class ProductService
    {
        private readonly string _connectionString;
        public ProductService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void SaveProducts(List<ProductDetail> products)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = @"INSERT INTO ProductDetails (Image, Title, Description, Quantity, Price, Date) 
                      VALUES (@Image, @Title, @Description, @Quantity, @Price, @Date)";

                using (var transaction = connection.BeginTransaction()) // Begin a transaction for bulk insert
                {
                    foreach (var product in products)
                    {
                        using (var command = new SqliteCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Image", product.ImageData);
                            command.Parameters.AddWithValue("@Title", product.Title);
                            command.Parameters.AddWithValue("@Description", product.Description);
                            command.Parameters.AddWithValue("@Quantity", product.Quantity);
                            command.Parameters.AddWithValue("@Price", product.Price);
                            command.Parameters.AddWithValue("@Date", product.Date);

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public List<ProductDetail> GetAllProducts()
        {
            var products = new List<ProductDetail>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT Id, Image, Title, Description, Quantity, Price, Date FROM ProductDetails";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new ProductDetail
                        {
                            Id = reader.GetInt32(0),
                            ImageData = reader.GetString(1),
                            Title = reader.GetString(2),
                            Description = reader.GetString(3),
                            Quantity = reader.GetInt32(4),
                            Price = reader.GetDouble(5),
                            Date = reader.GetDateTime(6)
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }
    }
}
