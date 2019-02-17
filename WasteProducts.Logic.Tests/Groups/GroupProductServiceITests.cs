﻿using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Services.Groups;

namespace WasteProducts.Logic.Tests.GroupManagementTests
{
    public class GroupProductServiceTests
    {
        private GroupProduct _groupProduct;
        private GroupProductDB _groupProductDB;
        private GroupBoardDB _groupBoardDB;
        private GroupUserDB _groupUserDB;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private GroupProductService _groupProductService;
        private IRuntimeMapper _mapper;
        private List<GroupBoardDB> _selectedBoardList;
        private List<GroupUserDB> _selectedUserList;
        private List<GroupProductDB> _selectedProductList;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {

        }
        [SetUp]
        public void TestCaseSetup()
        {
            _groupProductDB = new GroupProductDB
            {
                Id = "00000000-0000-0000-0000-000000000004",
                GroupBoardId = "00000000-0000-0000-0000-000000000003",
                Information = "Information",
                ProductId = "2"
            };
            _groupProduct = new GroupProduct
            {
                Id = "00000000-0000-0000-0000-000000000004",
                GroupBoardId = "00000000-0000-0000-0000-000000000003",
                Information = "Information",
                ProductId = "2"
            };
            _groupBoardDB = new GroupBoardDB
            {
                Id = "00000000-0000-0000-0000-000000000003",
                CreatorId = "2",
                Information = "Some product",
                Name = "Best",
                Created = DateTime.UtcNow,
                Deleted = null,
                IsNotDeleted = true,
                Modified = DateTime.UtcNow,
                GroupId ="00000000-0000-0000-0000-000000000001",
                GroupProducts = null
            };
            _groupUserDB = new GroupUserDB
            {
                GroupId = "00000000-0000-0000-0000-000000000001",
                RightToCreateBoards = true
            };
            _groupRepositoryMock = new Mock<IGroupRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GroupProfile());
                cfg.AddProfile(new GroupBoardProfile());
                cfg.AddProfile(new GroupProductProfile());
                cfg.AddProfile(new GroupUserProfile());
                cfg.AddProfile(new GroupCommentProfile());
            });

            _mapper = (new Mapper(config)).DefaultContext.Mapper;
            _groupProductService = new GroupProductService(_groupRepositoryMock.Object, _mapper);
            _selectedBoardList = new List<GroupBoardDB>();
            _selectedUserList = new List<GroupUserDB>();
            _selectedProductList = new List<GroupProductDB>();

        }

        [Test]
        public void GroupProductService_01_Create_01_Add_New_GroupProduct()
        {
            _selectedBoardList.Add(_groupBoardDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            var x = Task.Run(()=>_groupProductService.Create(_groupProduct)).Result;

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupProductDB>()), Times.Once);
        }
        [Test]
        public void GroupProductService_01_Create_02_Group_Unavalible_or_GroupUser_Unavalible_or_GroupBoard_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Assert.ThrowsAsync<ValidationException>(()=>
                        _groupProductService.Create(_groupProduct));
        }

        [Test]
        public void GroupProductService_02_Update_01_Update_Information_In_GroupProduct()
        {
            _selectedProductList.Add(_groupProductDB);

            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupProductDB, Boolean>>()))
                .ReturnsAsync(_selectedProductList);

            Task.Run(()=>_groupProductService.Update(_groupProduct)).Wait();

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupProductDB>()), Times.Once);
        }
        [Test]
        public void GroupProductService_02_Update_02_GroupBoard_Unavalible_or_UserGroup_Unavalible_or_Group_Unavalible_or_GroupProduct_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupProductDB, Boolean>>()))
                .ReturnsAsync(_selectedProductList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Assert.ThrowsAsync<ValidationException>(() =>
                        _groupProductService.Update(_groupProduct));
        }

        [Test]
        public void GroupProductService_03_Delete_01_Remove_GroupProduct()
        {
            _selectedProductList.Add(_groupProductDB);
            _selectedBoardList.Add(_groupBoardDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupProductDB, Boolean>>()))
                .ReturnsAsync(_selectedProductList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Task.Run(()=>_groupProductService.Delete("00000000-0000-0000-0000-000000000003")).Wait();

            _groupRepositoryMock.Verify(m => m.Delete(_groupProductDB), Times.Once);
        }
        [Test]
        public void GroupProductService_03_Delete_02_GroupBoard_Unavalible_or_UserGroup_Unavalible_or_Group_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupProductDB, Boolean>>()))
                .ReturnsAsync(_selectedProductList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Assert.ThrowsAsync<ValidationException>(() =>
                        _groupProductService.Delete("00000000-0000-0000-0000-000000000003"));
        }

        [Test]
        public void GroupProductService_04_FindById_01_Obtainment_Avalible_GroupProduct_By_Id()
        {
            _selectedProductList.Add(_groupProductDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupProductDB, Boolean>>()))
                .ReturnsAsync(_selectedProductList);

            var result = Task.Run(()=>_groupProductService.FindById("00000000-0000-0000-0000-000000000000")).Result;
            Assert.AreEqual(_groupProduct.Id, result.Id);
            Assert.AreEqual(_groupProduct.Information, result.Information);
        }
        [Test]
        public void GroupProductService_04_FindById_02_Obtainment_Unavalible_GroupBoard_By_Id()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupProductDB, Boolean>>()))
                .ReturnsAsync(_selectedProductList);

            var result = Task.Run(() => _groupProductService.FindById("00000000-0000-0000-0000-000000000000")).Result;
            Assert.AreEqual(null, result);
        }
    }
}
