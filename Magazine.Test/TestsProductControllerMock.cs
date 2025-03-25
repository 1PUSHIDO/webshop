using System;
using Magazine.Core.Models;
using Magazine.Core.Services;
using Magazine.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Magazine.Test
{
    class TestsProductControllerMock
    {
        private IProductService productService;
        private IConfiguration configuration;
        private Guid guid;
        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder().Build();
            productService = new ProductService(configuration);
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

            var mock = new Mock<IProductService>();
            mock.Setup(a => a.Add(product)).Returns(product);

            ProductController controller = new(mock.Object);

            var result = (IStatusCodeActionResult)controller.Post(product);

            Console.WriteLine($"Метод Post({result}): {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            //Assert.IsFalse(result == null);
        }

        [Test, Order(1)]
        public void GetTest()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(a => a.Search(guid)).Returns(new Product { Id = guid });
            
            ProductController controller = new(mock.Object);

            var result = (IStatusCodeActionResult)controller.Get(guid);

            //Product result = productService.Search(guid);

            Console.WriteLine($"Метод Get: {DateTime.Now:G} | {guid}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [TestCase("NewTestProductDefinition", "NewTestProductName", 200.0, "NewTestProductImage"), Order(2)]
        public void PutTest(string definition, string name, double price, string image)
        {
            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            var mock = new Mock<IProductService>();
            mock.Setup(a => a.Edit(product)).Returns(product);
            
            ProductController controller = new(mock.Object);

            var result = (IStatusCodeActionResult)controller.Put(product);

            Console.WriteLine($"Метод Put: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test, Order(3)]
        public void DeleteTest()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(a => a.Remove(guid)).Returns(new Product { Id = guid });

            ProductController controller = new(mock.Object);

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
