﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Services;
using System.Linq;
using WasteProducts.Logic.Common.Services;
using AutoMapper;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic.Tests.Search_Tests
{

    [TestFixture]
    public class SearchService_Test
    {
        [SetUp]
        public void Setup()
        {
            users = new List<TestUser>
            {
                new TestUser { Id = "1", Login = "user1", Email = "user1@mail.net" },
                new TestUser { Id = "2", Login = "user2", Email = "user2@mail.net" },
                new TestUser { Id = "3", Login = "user3", Email = "user3@mail.net" },
                new TestUser { Id = "4", Login = "user4", Email = "user4@mail.net" },
                new TestUser { Id = "5", Login = "user5", Email = "user5@mail.net" }
            };

            mockRepo = new Mock<ISearchRepository>();
            sut = new LuceneSearchService(mockRepo.Object, null, null);
        }

        private IEnumerable<TestUser> users;
        private Mock<ISearchRepository> mockRepo;
        private Mock<IMapper> mockMapper;
        private ISearchService sut;

        [Test]
        public void Search_EmptyQuery_Return_Exception()
        {
            mockRepo.Setup(x => x.GetAll<TestUser>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>())).Returns(users);

            var query = new BoostedSearchQuery();
            Assert.Throws<ArgumentException>(() => sut.Search<TestUser>(query));
        }

        [Test]
        public void Search_EmptyQuery_Return_ArgumentException()
        {
            mockRepo.Setup(x => x.GetAll<TestUser>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>())).Returns(users);

            var query = new BoostedSearchQuery();

            Assert.Throws<ArgumentException>(() => sut.Search<TestUser>(query));
        }

        [Test]
        public void AddIndex_InsertNewIndex_Return_NoException()
        {
            TestUser user = new TestUser();
            mockRepo.Setup(x => x.Insert<TestUser>(user));

            sut.AddToSearchIndex<TestUser>(user);
        }

        [Test]
        public void AddIndex_InsertNewIndexVerify_Return_Once()
        {
            var user = new TestUser();
            mockRepo.Setup(x => x.Insert<TestUser>(user)).Verifiable();

            sut.AddToSearchIndex<TestUser>(user);

            mockRepo.Verify(v => v.Insert<TestUser>(user), Times.Once);
        }

        [Test]
        public void AddIndex_InsertNewIndexIEnumerable_Return_NoException()
        {
            mockRepo.Setup(x => x.Insert<TestUser>(It.IsAny<TestUser>()));

            sut.AddToSearchIndex<TestUser>(users);
        }

        [Test]
        public void AddIndex_InsertNewIndexIEnumerableVerify_Return_UsersCount()
        {
            mockRepo.Setup(x => x.Insert<TestUser>(It.IsAny<TestUser>())).Verifiable();

            sut.AddToSearchIndex<TestUser>(users);

            mockRepo.Verify(v => v.Insert<TestUser>(It.IsAny<TestUser>()), Times.Exactly(users.Count<TestUser>()));
        }

        [Test]
        public void RemoveIndex_DeleteIndex_Return_NoException()
        {
            TestUser user = new TestUser();
            mockRepo.Setup(x => x.Delete<TestUser>(user));

            sut.RemoveFromSearchIndex<TestUser>(user);
        }

        [Test]
        public void RemoveIndex_DeleteIndexVerify_Return_Once()
        {
            var user = new TestUser();
            mockRepo.Setup(x => x.Delete<TestUser>(user)).Verifiable();

            sut.RemoveFromSearchIndex<TestUser>(user);

            mockRepo.Verify(v => v.Delete<TestUser>(user), Times.Once);
        }

        [Test]
        public void RemoveIndex_DeleteIndexIEnumerable_Return_NoException()
        {
            mockRepo.Setup(x => x.Delete<TestUser>(It.IsAny<TestUser>()));

            sut.RemoveFromSearchIndex<TestUser>(users);
        }

        [Test]
        public void RemoveIndex_DeleteIndexIEnumerableVerify_Return_UsersCount()
        {
            mockRepo.Setup(x => x.Delete<TestUser>(It.IsAny<TestUser>())).Verifiable();

            sut.RemoveFromSearchIndex<TestUser>(users);

            mockRepo.Verify(v => v.Delete<TestUser>(It.IsAny<TestUser>()), Times.Exactly(users.Count<TestUser>()));
        }

        [Test]
        public void UpdateIndex_UpdateIndex_Return_NoException()
        {
            TestUser user = new TestUser();
            mockRepo.Setup(x => x.Update<TestUser>(user));

            sut.UpdateInSearchIndex<TestUser>(user);
        }

        [Test]
        public void UpdateIndex_UpdateIndexVerify_Return_Once()
        {
            var user = new TestUser();
            mockRepo.Setup(x => x.Update<TestUser>(user)).Verifiable();

            sut.UpdateInSearchIndex<TestUser>(user);

            mockRepo.Verify(v => v.Update<TestUser>(user), Times.Once);
        }

        [Test]
        public void UpdateIndex_UpdateIndexIEnumerable_Return_NoException()
        {
            mockRepo.Setup(x => x.Update<TestUser>(It.IsAny<TestUser>()));

            sut.UpdateInSearchIndex<TestUser>(users);
        }

        [Test]
        public void UpdateIndex_UpdateIndexIEnumerableVerify_Return_UsersCount()
        {
            mockRepo.Setup(x => x.Update<TestUser>(It.IsAny<TestUser>())).Verifiable();

            sut.UpdateInSearchIndex<TestUser>(users);

            mockRepo.Verify(v => v.Update<TestUser>(It.IsAny<TestUser>()), Times.Exactly(users.Count<TestUser>()));
        }

        [Test]
        public void ClearIndex_ClearIndex_Return_NoException()
        {
            mockRepo.Setup(x => x.Clear());

            sut.ClearSearchIndex();
        }

        [Test]
        public void ClearIndex_ClearIndexVerify_Return_Once()
        {
            mockRepo.Setup(x => x.Clear()).Verifiable();

            sut.ClearSearchIndex();

            mockRepo.Verify(v => v.Clear(), Times.Once);
        }

        [Test]
        public void OptimizeIndex_OptimizeIndex_Return_NoException()
        {
            mockRepo.Setup(x => x.Optimize());

            sut.OptimizeSearchIndex();
        }

        [Test]
        public void OptimizeIndex_OptimizeIndexVerify_Return_Once()
        {
            mockRepo.Setup(x => x.Optimize()).Verifiable();

            sut.OptimizeSearchIndex();

            mockRepo.Verify(v => v.Optimize(), Times.Once);
        }
    }
}
