﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using Newtonsoft.Json.Linq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.Logic.Services;
using WasteProducts.Web.Controllers.Api;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Web.Tests.WebApiTests
{
    [TestFixture]
    public class SearchWebApiTest
    {
        private IEnumerable<ProductDB> products;

        [OneTimeSetUp]
        public void Setup()
        {
            products = new List<ProductDB>
            {
                new ProductDB { Name = "Test Product1 Name1", Composition = "Test Product1 Composition1"},
                new ProductDB { Name = "Test Product2 Name2", Composition = "Test Product2 Composition2"},
                new ProductDB { Name = "Test Product3 Name3", Composition = "Test Product3 Composition3"},
                new ProductDB { Name = "Test Product4 Name4", Composition = "Test Product4 Composition4"},
                new ProductDB { Name = "Test Product5 Name5 Unique", Composition = "Test Product5 Composition5"}
            };

        }

        [Test]
        public async Task SearchControllerTestGetProductWithDefaultFields()
        {
            Mock<ILogger> _mockLogger = new Mock<ILogger>();
            LuceneSearchRepository repo = new LuceneSearchRepository(true);
            LuceneSearchService service = new LuceneSearchService(repo, null, null);
            SearchController sut = new SearchController(service, _mockLogger.Object);

            service.AddToSearchIndex<ProductDB>(products);

            var result = await sut.GetProductsDefaultFields("test") as OkNegotiatedContentResult<IEnumerable<Product>>;
            Assert.AreEqual(expected: 5, actual: result.Content.Count());

        }
    }
}
