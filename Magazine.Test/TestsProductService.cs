using System.Xml.Linq;
using Magazine.Core.Models;
using Magazine.Core.Services;
using Magazine.WebApi;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace Magazine.Test
{
    public class TestsProductService
    {
        private IProductService productService;
        private readonly Guid guid = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            productService = new ProductService(new ConfigurationBuilder().Build());
            Console.WriteLine($"Тест класса запущен: {DateTime.Now:G} | GUID - {guid}");
        }

        [TestCase("TestProductDefinition", "TestProductName", 100.0, "TestProductImage"), Order(0)]
        public void AddTest(string definition, string name, double price, string image)
        {
            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            Product result = productService.Add(product);
            
            Console.WriteLine($"Метод Add: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");
            Assert.Catch(() =>
            {
                productService.Add(product);
            });
            //Assert.That(result, Is.EqualTo(product));
        }

        [Test, Order(1)]
        public void SearchTest()
        {
            Product result = productService.Search(guid);

            Console.WriteLine($"Метод Search: {DateTime.Now:G}\n{{ {guid}\n {result.Name}\n {result.Definition}\n {result.Price}\n {result.Image} }}");
            Assert.That(result.Id, Is.EqualTo(guid));
        }

        [TestCase("NewTestProductDefinition", "NewTestProductName", 200.0, "NewTestProductImage"), Order(2)]
        public void EditTest(string definition, string name, double price, string image)
        {
            Product product = new()
            {
                Id = guid,
                Definition = definition,
                Name = name,
                Price = price,
                Image = image
            };

            Product result = productService.Edit(product);

            Console.WriteLine($"Метод Edit: {DateTime.Now:G}\n{{ {guid}\n {name}\n {definition}\n {price}\n {image} }}");
            Assert.That(result, Is.EqualTo(product));
        }

        [Test, Order(3)]
        public void RemoveTest()
        {
            Product result = productService.Remove(guid);

            Console.WriteLine($"Метод Remove: {DateTime.Now:G}\n{{ {guid}\n {result.Name}\n {result.Definition}\n {result.Price}\n {result.Image} }}");
            Assert.That(result.Id, Is.EqualTo(guid));
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine($"Тест класса завершен: {DateTime.Now.ToString("G")}");
        }
    }
}