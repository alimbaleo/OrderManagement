using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entitities;
using OrderManagement.EntityFramework.Identity;
using static OrderManagement.Domain.Constants;
namespace OrderManagement.EntityFramework
{
    public class OrderManagementDBContext : IdentityDbContext<AppUser>
    {
        public OrderManagementDBContext(DbContextOptions<OrderManagementDBContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(b =>
            {
                b.ToTable("AppUsers");
                b.HasData(GetDefaultAdminUser());
            });

            builder.Entity<IdentityRole>(i =>
            {
                i.ToTable("Roles");
                i.HasData(new IdentityRole { Id = USER_ROLE, Name = USER_ROLE, NormalizedName = USER_ROLE, ConcurrencyStamp = Guid.NewGuid().ToString() }, new IdentityRole { Id = ADMIN_ROLE, Name = ADMIN_ROLE, NormalizedName = ADMIN_ROLE, ConcurrencyStamp = Guid.NewGuid().ToString() });
            });

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE,
                UserId = DEFAULT_ADMIN_EMAIL
            });
        }
        private AppUser GetDefaultAdminUser()
        {
            var appUser = new AppUser
            {
                Id = DEFAULT_ADMIN_EMAIL,
                Email = DEFAULT_ADMIN_EMAIL,
                EmailConfirmed = true,
                FirstName = "Admin",
                Surname = "Admin",
                UserName = DEFAULT_ADMIN_EMAIL,
            NormalizedUserName = DEFAULT_ADMIN_EMAIL.ToUpper()
            };

            //set user password
            PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, DEFAULT_ADMIN_PASSWORD);

            return appUser;
        }
    }
}