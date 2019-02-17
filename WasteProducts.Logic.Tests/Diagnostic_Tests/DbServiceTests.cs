﻿using System.Threading.Tasks;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Services;

namespace WasteProducts.Logic.Tests.Diagnostic_Tests
{
    [TestFixture]
    public class DbServiceTests
    {
        private const string IncorrectMethodWorkMsg = "Method works incorrect";
        private Mock<ILogger> _loggerMoq;
        private Mock<IDatabase> _databaseMoq;
        private Mock<IDiagnosticRepository> _diagRepoMoq;
        private Mock<IProductService> _prodServiceMoq;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _loggerMoq = new Mock<ILogger>();
            _databaseMoq = new Mock<IDatabase>();
            _diagRepoMoq = new Mock<IDiagnosticRepository>();
            _prodServiceMoq = new Mock<IProductService>();
        }

        [TearDown]
        public void TearDown()
        {
            _loggerMoq.Reset();
            _databaseMoq.Reset();
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void GetStatus_Returns_DatabaseStatus_When(bool databaseIsExists, bool databaseIsCompatibleWithModel)
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(databaseIsExists);
            _databaseMoq.SetupGet(database => database.IsCompatibleWithModel).Returns(databaseIsCompatibleWithModel);

            var expectedResult = new DatabaseState(databaseIsExists, databaseIsCompatibleWithModel);

            //action
            var actualResult = dbManagementService.GetStateAsync().Result;

            // assert
            Assert.AreEqual(expectedResult.IsExist, actualResult.IsExist, IncorrectMethodWorkMsg);
            Assert.AreEqual(expectedResult.IsCompatibleWithModel, actualResult.IsCompatibleWithModel, IncorrectMethodWorkMsg);

            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);

            if (databaseIsExists)
            {
                _databaseMoq.VerifyGet(database => database.IsCompatibleWithModel, Times.Once);

                if (!databaseIsCompatibleWithModel)
                    _loggerMoq.Verify(logger => logger.Warn(It.IsAny<string>()), Times.Once);
            }
            else
                _databaseMoq.VerifyGet(database => database.IsCompatibleWithModel, Times.Never);
        }

        [Test]
        public async Task DeleteAsync_Test()
        {
            // arrange
            var dbManagementService = GetDbService();

            // action
            await dbManagementService.DeleteAsync().ConfigureAwait(false);

            // assert
            _databaseMoq.Verify(database => database.Delete(), Times.Once);
        }

        [Test]
        public async Task ReCreateAsync_Test()
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(false);

            // action
           await dbManagementService.RecreateAsync().ConfigureAwait(false);

            // assert
            _diagRepoMoq.Verify(s => s.RecreateAsync(), Times.Once);
        }

        IDbService GetDbService() => new DbService(_diagRepoMoq.Object, _databaseMoq.Object, _prodServiceMoq.Object, _loggerMoq.Object);
    }
}
