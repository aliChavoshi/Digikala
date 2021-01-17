using System.Linq;
using Digikala.DataAccessLayer.Entities;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.EntityFrameworkCore;

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
                .HasQueryFilter(u => u.IsDeleted== false);

            modelBuilder.Entity<Permission>()
                .HasQueryFilter(u => u.IsDeleted == false);

            modelBuilder.Entity<Store>()
                .HasQueryFilter(u => u.IsDeleted == false);
        }
    }
}