using Magazine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Magazine.WebApi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        private readonly string _connectionString;


    }
}
