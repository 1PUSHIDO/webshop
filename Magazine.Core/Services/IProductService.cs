using Magazine.Core.Models;

namespace Magazine.Core.Services
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Add product to DB
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Product</returns>
        Product Add(Product product);

        /// <summary>
        /// Remove product from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        Product Remove(Guid id);

        /// <summary>
        /// Edit product in DB
        /// </summary>
        /// <param name="updatedProduct"></param>
        /// <returns>Product</returns>
        Product Edit(Product updatedProduct);

        /// <summary>
        /// Search product in DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        Product Search(Guid id);
    }
}
