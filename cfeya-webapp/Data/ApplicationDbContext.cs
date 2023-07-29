using cfeya_webapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace cfeya_webapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Wishes> TblWish { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
            //builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            //For Adding Roles of the Users in Hard coded Form.
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Fairy",
                    NormalizedName = "Fairy"
                }, new IdentityRole
                {
                    Id = "2",
                    Name = "Cinderella",
                    NormalizedName = "Cinderella"
                });
        }
    }
}



public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(a => a.Firstname).HasMaxLength(225);
        builder.Property(a => a.Lastname).HasMaxLength(225);
        builder.Property(a => a.Middlename).HasMaxLength(225);
        builder.Property(a => a.Gender).HasMaxLength(10);
        builder.Property(a => a.CellPhone).HasMaxLength(50);
        builder.Property(a => a.HomePhone).HasMaxLength(50);
        builder.Property(a => a.WorkPhone).HasMaxLength(50);
        builder.Property(a => a.Street).HasMaxLength(50);
        builder.Property(a => a.City).HasMaxLength(100);
        builder.Property(a => a.State).HasMaxLength(50);

    }
}