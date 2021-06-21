using System;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DataAccessLayer.SeedingData;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Digikala.DataAccessLayer.Context
{
    public class DigikalaContext : DbContext
    {
        public DigikalaContext(DbContextOptions<DigikalaContext> options) : base(options)
        {

        }

        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Store> Stores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => u.IsDeleted == false);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(u => u.IsDeleted == false);

            modelBuilder.Entity<Permission>()
                .HasQueryFilter(u => u.IsDeleted == false);

            modelBuilder.Entity<Store>()
                .HasQueryFilter(u => u.IsDeleted == false);

            modelBuilder.Entity<RolePermission>()
                .HasQueryFilter(x =>
                    x.ExpireRolePermission.HasValue ? x.ExpireRolePermission.Value.Date >= DateTime.Now.Date : !x.ExpireRolePermission.HasValue);

            //Seeding Data
            DataSeeder.SeedRoles(modelBuilder);
            DataSeeder.SeedUserAdmin(modelBuilder);
            DataSeeder.SeedPermission(modelBuilder);
            DataSeeder.SeedRolePermission(modelBuilder);
        }
    }
}