﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Contexts.Security.Configurations
{
    class RoleConfiguration : EntityTypeConfiguration<IRoleDb>
    {
        public RoleConfiguration()
        {
            ToTable("Roles");
           
            HasKey(c => c.Id)
              .Property(c => c.Id)
              .HasColumnName("RoleId")
              .HasColumnType("int")
              .IsRequired();

            Property(c => c.Name)
               .HasColumnName("Name")
               .HasColumnType("nvarchar")
               .HasMaxLength(256)
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("NameIndex") { IsUnique = true }));

            HasMany(c => c.Users)
                .WithMany(c => c.Roles)
                .Map(c =>
                {
                    c.ToTable("UsersRoles");
                    c.MapLeftKey("RoleId");
                    c.MapRightKey("UserId");
                });


        }
    }
}
