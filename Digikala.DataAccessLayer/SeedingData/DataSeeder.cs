using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.Utility.Generator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Digikala.DataAccessLayer.SeedingData
{
    public class DataSeeder
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new List<Role>
            {
                new Role() {Id = 1, IsDeleted = false, Title = "کاربر عادی"},
                new Role() {Id = 2, IsDeleted = false, Title = "فروشندگان"},
                new Role() {Id = 3, IsDeleted = false, Title = "ادمین"},
            });
        }
        public static void SeedPermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(new List<Permission>()
            {
                new Permission()
                {
                    Id = 1,
                    IsDeleted = false,
                    Name = "صفحه اصلی فروشگاه",
                    ParentId = null
                },
                new Permission()
                {
                    Id = 2,
                    IsDeleted = false,
                    Name = "صفحه اصلی ادمین",
                    ParentId = null
                }
            });
        }
        public static void SeedRolePermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>().HasData(new List<RolePermission>()
            {
                new RolePermission()
                {
                    Id = 1,
                    RoleId = 2,
                    PermissionId = 1
                },
                new RolePermission()
                {
                    Id = 2,
                    RoleId = 3,
                    PermissionId = 1
                },
                new RolePermission()
                {
                    Id = 3,
                    RoleId = 3,
                    PermissionId = 2
                }
            });
        }
        public static void SeedUserAdmin(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User()
                {
                    Email = "alichavoshii1372@gmail.com",
                    ConfirmEmail = true,
                    ConfirmMobile = true,
                    CreateDate = DateTime.Now,
                    Fullname = "ادمین",
                    IsActive = true,
                    Mobile = "09130242780",
                    Password = HashGenerators.Encrypt("admin"),
                    Version = 0,
                    RoleId = 3,
                    IsDeleted = false,
                    Id = 1
                }
            });
        }
    }
}