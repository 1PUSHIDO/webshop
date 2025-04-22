using System;
using Magazine.Core.Models;
using Magazine.Core.Services;
using Magazine.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Magazine.Test
{
    public class TestsProductController
    {
        private IProductService productService;
        private IConfiguration configuration;
        private Guid guid;
        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder().Build();
            productService = new ProductService(configuration, new Database());
            Console.WriteLine($"���� ����������� �������: {DateTime.Now:G}");
        }

        [TestCase("TestProductDefinition", "TestProductName", 100.0, "TestProductImage"), Order(0)]
        public void PostTest(string definition, string name, double price, string image)
        {
            ProductController controller = new(productService);

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

            Console.WriteLine($"����� Post({result}): {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            //Assert.IsFalse(result == null);
        }

        [Test, Order(1)]
        public void GetTest()
        {
            ProductController controller = new(productService);

            var result = (IStatusCodeActionResult)controller.Get(guid);

            //Product result = productService.Search(guid);

            Console.WriteLine($"����� Get: {DateTime.Now:G} | {guid}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [TestCase("NewTestProductDefinition", "NewTestProductName", 200.0, "NewTestProductImage"), Order(2)]
        public void PutTest(string definition, string name, double price, string image)
        {
            ProductController controller = new(productService);

            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            var result = (IStatusCodeActionResult)controller.Put(product);

            Console.WriteLine($"����� Put: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test, Order(3)]
        public void DeleteTest()
        {
            ProductController controller = new(productService);

            var result = (IStatusCodeActionResult)controller.Delete(guid);

            Console.WriteLine($"����� Delete: {DateTime.Now:G} | {guid}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine($"���� ����������� ��������: {DateTime.Now.ToString("G")}");
        }
    }
}