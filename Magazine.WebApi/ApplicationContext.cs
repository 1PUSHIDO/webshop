using Magazine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Magazine.WebApi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        private readonly string _connectionString;

        public ApplicationContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DataBaseFilePath");
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }


    }
}
