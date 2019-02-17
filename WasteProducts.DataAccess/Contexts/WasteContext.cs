﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Models.Notifications;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Contexts.Config;
using WasteProducts.DataAccess.ModelConfigurations;
using WasteProducts.DataAccess.ModelConfigurations.Donations;
using WasteProducts.DataAccess.ModelConfigurations.Notifications;
using WasteProducts.DataAccess.ModelConfigurations.Users;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext<UserDB, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        private readonly ISearchRepository _searchRepository;

        public WasteContext(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
            Database.Log = (s) => Debug.WriteLine(s);
        }

        public WasteContext(string nameOrConnectionString, ISearchRepository searchRepository) : base(nameOrConnectionString)
        {
            _searchRepository = searchRepository;
            Database.Log = (s) => Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ComplexType<NotificationSettingsDB>();

            modelBuilder.Entity<UserDB>().HasMany(u => u.Friends).WithMany()
                .Map(t => t.MapLeftKey("UserId").MapRightKey("FriendId").ToTable("UserFriends"));

            modelBuilder.Entity<UserDB>().HasMany(u => u.Notifications).WithRequired(n => n.User);

            modelBuilder.Entity<ProductDB>()
                .HasOptional(b => b.Barcode)
                .WithOptionalDependent(p => p.Product);

            modelBuilder.Configurations.Add(new UserProductDescriptionConfiguration());
            modelBuilder.Configurations.Add(new NewEmailConfirmatorConfiguration());

            modelBuilder.Configurations.Add(new GroupBoardConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new GroupUserConfiguration());
            modelBuilder.Configurations.Add(new GroupCommentConfiguration());
            modelBuilder.Configurations.Add(new GroupProductConfiguration());

            modelBuilder.Configurations.Add(new DonationEntityConfiguration());
            modelBuilder.Configurations.Add(new DonorEntityConfiguration());
            modelBuilder.Configurations.Add(new AddressEntityConfiguration());

            modelBuilder.Configurations.Add(new NotificationConfiguration());
        }

        /// <summary>
        /// Entity for changing email.
        /// </summary>
        public IDbSet<NewEmailConfirmator> NewEmailConfirmators { get; set; }

        /// <summary>
        /// Entity represents many-to-many relationship between User and Product and includes ratings and descriptions of products by specific users.
        /// </summary>
        public IDbSet<UserProductDescriptionDB> UserProductDescriptions { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get product list operations in 'ProductRepository' class.
        /// </summary>
        public IDbSet<ProductDB> Products { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get barcode list operations in 'BarcodeRepository' class.
        /// </summary>
        public IDbSet<BarcodeDB> Barcodes { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get category list operations in 'CategoryRepository' class.
        /// </summary>
        public IDbSet<CategoryDB> Categories { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        /// CRUD operations in 'DonationRepository' class.
        /// </summary>
        public IDbSet<DonationDB> Donations { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        /// CRUD operations in 'DonationRepository' class.
        /// </summary>
        public IDbSet<DonorDB> Donors { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        /// CRUD operations in 'DonationRepository' class.
        /// </summary>
        public IDbSet<AddressDB> Addresses { get; set; }

        public IDbSet<GroupBoardDB> GroupBoards { get; set; }
        public IDbSet<GroupDB> Groups { get; set; }
        public IDbSet<GroupUserDB> GroupUsers { get; set; }
        public IDbSet<GroupCommentDB> GroupComments { get; set; }
        public IDbSet<GroupProductDB> GroupProducts { get; set; }

        public IDbSet<NotificationDB> Notification { get; set; }

        public override int SaveChanges()
        {
            SaveChangesToSearchRepository();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            SaveChangesToSearchRepository();
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Save changes to Lucene search repository.
        /// </summary>
        private void SaveChangesToSearchRepository()
        {
            DetectAndSaveChanges(typeof(ProductDB));
        }

        /// <summary>
        /// Detectes changes and save it to Lucene using LuceneSearchRepository
        /// </summary>
        /// <param name="types">Object type that needed to detect and save</param>
        protected void DetectAndSaveChanges(params Type[] types)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (!types.Contains(entry.Entity.GetType()))
                {
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        _searchRepository.Insert(entry.Entity); break;
                    case EntityState.Modified:
                        _searchRepository.Update(entry.Entity); break;
                    case EntityState.Deleted:
                        _searchRepository.Delete(entry.Entity); break;
                }
            }
        }

        /// <summary>
        /// Method not for use, just for preventing some bug made by .NET "optimization"
        /// </summary>
        private void FixEfProviderServicesProblem()
        {
            // This method prevents this bug:
            // The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            // for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            // Make sure the provider assembly is available to the running application. 
            // See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
