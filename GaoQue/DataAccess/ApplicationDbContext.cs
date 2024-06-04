using GaoQue.Models;
using GaoQue.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaoQue.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<ProductImage> ProductImages { get; set; }
            public DbSet<ProductInformation> productInformation { get; set; }
            public DbSet<Menu> Menus { get; set; }
            public DbSet<Slider> Sliders { get; set; }
            public DbSet<Contact> Contacts { get; set; }
            public DbSet<Banner> Banners { get; set; }
            public DbSet<CartItem> CartItems { get; set; }  
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetail> OrderDetails { get; set; }
            public DbSet<GPTData> GPTDatas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");

            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");

            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");

            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        }
    }
}

