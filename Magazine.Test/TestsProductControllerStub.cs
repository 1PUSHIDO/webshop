using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magazine.Core.Models;
using Magazine.Core.Services;
using Magazine.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;

internal class TestProductController : Controller
{
    private Product product;
    
    public IActionResult Get(Guid id)
    {
        if (product is null)
            return NotFound();
        return Ok(product);
    }

    public IActionResult Post(Product product)
    {
        this.product = product;
        return CreatedAtAction(nameof(Get), new { id = this.product.Id }, this.product);
    }

    public IActionResult Put(Product updatedProduct)
    {
        if (product is null)
            return NotFound();
        this.product = updatedProduct;
        return Ok(this.product);
    }

    public IActionResult Delete(Guid id)
    {
        if (product is null)
            return NotFound();

        return Ok(product);
    }
}

namespace Magazine.Test
{
    class TestsProductControllerStub
    {
        TestProductController controller = new();
        private IConfiguration configuration;
        private Guid guid;
        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder().Build();
            Console.WriteLine($"Тест контроллера запущен: {DateTime.Now:G}");
        }

        [TestCase("TestProductDefinition", "TestProductName", 100.0, "TestProductImage"), Order(0)]
        public void PostTest(string definition, string name, double price, string image)
        {
            guid = Guid.NewGuid();

            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            var result = (IStatusCodeActionResult)controller.Post(product);

            Console.WriteLine($"Метод Post({result}): {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            //Assert.IsFalse(result == null);
        }

        [Test, Order(1)]
        public void GetTest()
        {
            //TestProductController controller = new();

            var result = (IStatusCodeActionResult)controller.Get(guid);

            //Product result = productService.Search(guid);

            Console.WriteLine($"Метод Get: {DateTime.Now:G} | {guid}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [TestCase("NewTestProductDefinition", "NewTestProductName", 200.0, "NewTestProductImage"), Order(2)]
        public void PutTest(string definition, string name, double price, string image)
        {
            //TestProductController controller = new();

            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            var result = (IStatusCodeActionResult)controller.Put(product);

            Console.WriteLine($"Метод Put: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test, Order(3)]
        public void DeleteTest()
        {
            //TestProductController controller = new();

            var result = (IStatusCodeActionResult)controller.Delete(guid);

            Console.WriteLine($"Метод Delete: {DateTime.Now:G} | {guid}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine($"Тест контроллера завершен: {DateTime.Now.ToString("G")}");
        }
    }
}
