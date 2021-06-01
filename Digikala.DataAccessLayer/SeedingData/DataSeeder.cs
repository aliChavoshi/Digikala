﻿using System;
using System.Collections.Generic;
using Digikala.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.Utility.Generator;

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
                }
            });
        }
        public static void SeedUserAdmin(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User()
                {
                    Email = "Admin@gmail.com",
                    ConfirmEmail = true,
                    ConfirmMobile = true,
                    CreateDate = DateTime.Now,
                    Fullname = "Admin",
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