using Magazine.Core.Models;
using Magazine.Core.Services;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace Magazine.WebApi
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString = $"Data Source=some.db;";
        private IConfiguration _configuration;
        private Dictionary<Guid, Product> map;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;

            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            SqliteCommand command = new()
            {
                Connection = connection,
                CommandText = @"CREATE TABLE IF NOT EXISTS Products (Id TEXT PRIMARY KEY,
                                                                     Name TEXT NOT NULL,
                                                                     Definition TEXT,
                                                                     Price REAL NOT NULL,
                                                                     Image TEXT);"
            };

            command.ExecuteNonQuery();

            InitFromFile();
        }

        private void InitFromFile() {
            string filePath = _configuration.GetValue<string>("DataBaseFilePath");
            if (filePath == null)
                filePath = "C:\\Users\\user\\Documents\\repos\\webshop\\Magazine.WebApi\\database.txt";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                map = JsonConvert.DeserializeObject<Dictionary<Guid, Product>>(json);

            } else
                map = new Dictionary<Guid, Product>();
        }

        private void WriteToFile() {
            string filePath = _configuration.GetValue<string>("DataBaseFilePath");
            if (filePath == null)
                filePath = "C:\\Users\\user\\Documents\\repos\\webshop\\Magazine.WebApi\\database.txt";

            string json = JsonConvert.SerializeObject(map);
            File.WriteAllText(filePath, json);
        }

        /// <inheritdoc/>
        public Product Add(Product product)
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            SqliteCommand command = new()
            {
                Connection = connection,
                CommandText = @"INSERT INTO Products (Id, Name, Definition, Price, Image)
                                VALUES (@Id, @Name, @Definition, @Price, @Image);"
            };
            command.Parameters.AddWithValue("@Id", product.Id);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Definition", product.Definition);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Image", product.Image);

            command.ExecuteNonQuery();

            map.Add(product.Id, product);
            WriteToFile();

            return product;
        }

        /// <inheritdoc/>
        public Product Remove(Guid id)
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            Product product = Search(id);
            if (product == null) return null;

            SqliteCommand command = new()
            {
                Connection = connection,
                CommandText = @"DELETE FROM Products
                                WHERE Id = @Id;"
            };
            command.Parameters.AddWithValue("@Id", product.Id);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0) return null;

            map.Remove(product.Id);
            WriteToFile();

            return product;
        }

        /// <inheritdoc/>
        public Product Edit(Product updatedProduct)
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            SqliteCommand command = new()
            {
                Connection = connection,
                CommandText = @"UPDATE Products SET Name = @Name,
                                                Definition = @Definition,
                                                Price = @Price,
                                                Image = @Image
                                WHERE Id = @Id;"
            };
            command.Parameters.AddWithValue("@Id", updatedProduct.Id);
            command.Parameters.AddWithValue("@Name", updatedProduct.Name);
            command.Parameters.AddWithValue("@Definition", updatedProduct.Definition);
            command.Parameters.AddWithValue("@Price", updatedProduct.Price);
            command.Parameters.AddWithValue("@Image", updatedProduct.Image);

            command.ExecuteNonQuery();

            map[updatedProduct.Id] = updatedProduct;
            WriteToFile();

            return updatedProduct;
        }

        /// <inheritdoc/>
        public Product Search(Guid id)
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            SqliteCommand command = new()
            {
                Connection = connection,
                CommandText = @"SELECT * FROM Products WHERE Id = @Id;"
            };
            command.Parameters.AddWithValue("@Id", id);

            SqliteDataReader dataReader = command.ExecuteReader();
            if (!dataReader.HasRows) return null;

            dataReader.Read();
            Product product = new()
            {
                Id = id,
                Name = dataReader.GetString(1),
                Definition = dataReader.GetString(2),
                Price = dataReader.GetDouble(3),
                Image = dataReader.GetString(4)
            };

            connection.Close();
            return product;
        }
    }
}
