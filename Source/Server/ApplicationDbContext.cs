using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Refactorizando.Shared.Data.Models;

namespace Refactorizando.Server
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var roleAdminId = "60c51d0e-6c24-4191-970b-aefb93fb9c51";
            var roleAdmin = new IdentityRole()
            {
                Id = roleAdminId,
                Name = "admin",
                NormalizedName = "admin"
            };
            var roleUserId = "adce8478-150e-4f7a-8dff-22bf15a0be0f";
            var roleUser = new IdentityRole()
            {
                Id = roleUserId,
                Name = "user",
                NormalizedName = "user"
            };
            modelBuilder.Entity<IdentityRole>().HasData(roleAdmin);
            modelBuilder.Entity<IdentityRole>().HasData(roleUser);
           
            modelBuilder.Entity<LikeRequest>(entity => {
                entity.HasKey(p =>  new {p.RequestId, p.SystemUserId});
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<LikeRequest> LikeRequests { get; set; }
    }
}