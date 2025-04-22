using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Magazine.Core.Models
{
    [Index("Id", IsUnique = true)]
    /// <summary>
    /// Information about product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique product ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product definition/description
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Product image
        /// </summary>
        public string Image { get; set; }
    }
}
