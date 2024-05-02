using Microsoft.EntityFrameworkCore;
using Syrophage.Models;

namespace Syrophage.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Contact> contacttb { get; set; }

        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Admin> Admins { get; set; }    

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }


        public DbSet<UserCoupon> UserCoupons { get; set; }









        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, email = "admin@gmail.com", role = "Admin" });

            modelBuilder.Entity<Admin>().HasData(new Admin { Id = 1, Email = "admin@gmail.com", Password = "Admin@123" });
        }

    }
}
