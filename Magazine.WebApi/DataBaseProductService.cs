using Magazine.Core.Models;
using Magazine.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Magazine.WebApi
{
    public class DataBaseProductService : IProductService
    {
        private readonly ApplicationContext _context;
        
        public DataBaseProductService(ApplicationContext context)
        {
            _context = context;
        }

        public Product Add(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product Edit(Product updatedProduct)
        {
            if (!_context.Products.Any(predicate => predicate.Id == updatedProduct.Id))
                return null;

            _context.Entry(updatedProduct).State = EntityState.Modified;
            _context.SaveChanges();

            return updatedProduct;
        }

        public Product Remove(Guid id)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct is null)
                return null;

            _context.Products.Remove(existingProduct);
            _context.SaveChanges();
            return existingProduct;
        }

        public Product Search(Guid id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
