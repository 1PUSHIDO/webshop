using Magazine.Core.Models;
using Magazine.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Magazine.WebApi
{
    /// <summary>
    /// Provides endpoints for interacting with products
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        //private IProductService _productService = new ProductService(new ConfigurationBuilder().Build());

        /// <summary>
        /// Get product from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var product = _productService.Search(id);
            if (product is null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Add product to DB
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Product</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            var createdProduct = _productService.Add(product);

            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Edit product from DB
        /// </summary>
        /// <param name="updatedProduct"></param>
        /// <returns>Product</returns>
        [HttpPut]
        public IActionResult Put([FromBody] Product updatedProduct)
        {
            var editedProduct = _productService.Edit(updatedProduct);

            if (editedProduct is null)
                return NotFound();

            return Ok(editedProduct);
        }

        /// <summary>
        /// Delete product from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var removedProduct = _productService.Remove(id);
            if (removedProduct is null)
                return NotFound();

            return Ok(removedProduct);
        }
    }
}
