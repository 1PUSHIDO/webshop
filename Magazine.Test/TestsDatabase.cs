using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magazine.Core.Models;
using Magazine.Core.Services;
using Magazine.WebApi;
using Microsoft.Extensions.Configuration;

namespace Magazine.Test
{
    class TestsDatabase
    {
        private Database database;
        private readonly Guid guid = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            database = new Database();
            database.Create();
            Console.WriteLine($"Тест класса запущен: {DateTime.Now:G} | GUID - {guid}");
        }

        [TestCase("TestProductDefinition", "TestProductName", 100.0, "TestProductImage"), Order(0)]
        public void InsertTest(string definition, string name, double price, string image)
        {
            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            //Product result = productService.Add(product);

            Console.WriteLine($"Метод Insert: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");
            //Assert.Catch(() =>
            //{
            Product result = database.Insert(product);
            //});
            Assert.That(result, Is.EqualTo(product));
        }

        [Test, Order(1)]
        public void SelectTest()
        {
            Product result = database.Select(guid);

            Console.WriteLine($"Метод Select: {DateTime.Now:G}\n{{ {guid}\n {result.Name}\n {result.Definition}\n {result.Price}\n {result.Image} }}");
            Assert.That(result.Id, Is.EqualTo(guid));
        }

        [TestCase("NewTestProductDefinition", "NewTestProductName", 200.0, "NewTestProductImage"), Order(2)]
        public void UpdateTest(string definition, string name, double price, string image)
        {
            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            Product result = database.Update(product);

            Console.WriteLine($"Метод Update: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");
            Assert.That(result, Is.EqualTo(product));
        }

        [Test, Order(3)]
        public void DeleteTest()
        {
            Product result = database.Delete(guid);

            Console.WriteLine($"Метод Delete: {DateTime.Now:G}\n{{ {guid}\n {result.Name}\n {result.Definition}\n {result.Price}\n {result.Image} }}");
            Assert.That(result.Id, Is.EqualTo(guid));
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine($"Тест класса завершен: {DateTime.Now.ToString("G")}");
        }
    }
}
