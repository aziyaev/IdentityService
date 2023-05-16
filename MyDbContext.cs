using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService
{
    public class MyDbContext : DbContext
    {
        private readonly AppSettings appSettings;
        public DbSet<User> Users { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options, IOptions<AppSettings> appSettings) : base(options)
        {
            this.appSettings = appSettings.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id).ValueGeneratedNever();

                entity.OwnsOne(u => u.Username, username => 
                {
                    username.Property(u => u.Value)
                        .HasColumnName("Username")
                        .HasMaxLength(appSettings.MaxUsernameLenght)
                        .IsRequired(); 
                });
                entity.OwnsOne(u => u.Role, role =>
                {
                    role.Property(u => u.Value)
                        .HasColumnName("Role")
                        .HasMaxLength(appSettings.MaxRoleLenght)
                        .IsRequired();
                });
                entity.OwnsOne(u => u.Email, email =>
                {
                    email.Property(u => u.Value)
                        .HasColumnName("Email")
                        .HasMaxLength(255)
                        .IsRequired();
                });
                entity.OwnsOne(u => u.PhoneNumber, phoneNumber =>
                {
                    phoneNumber.Property(u => u.Value)
                        .HasColumnName("PhoneNumber")
                        .HasMaxLength(20)
                        .IsRequired();
                });
                entity.OwnsOne(u => u.HashedPassword, hashedPassword =>
                {
                    hashedPassword.Property(u => u.Value)
                        .HasColumnName("HashedPassword")
                        .HasMaxLength(appSettings.MaxPasswordLenght)
                        .IsRequired();
                });
                entity.Property(u => u.CreatedTime)
                    .HasColumnName("CreatedTime")
                    .HasColumnType("datetime");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
