using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackAPI.Models;

namespace PruebaTecnicaBackAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.IsActive)
                      .IsRequired()
                      .HasDefaultValue(true);

                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(256)
                      .HasColumnName("PasswordHash");

                entity.HasMany(u => u.Addresses)
                      .WithOne(a => a.User)
                      .HasForeignKey(a => a.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");

                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(a => a.Street)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(a => a.City)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.Country)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.ZipCode)
                      .HasMaxLength(20);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currencies");

                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(c => c.Code)
                      .IsRequired()
                      .HasMaxLength(3)
                      .IsFixedLength();

                entity.HasIndex(c => c.Code)
                      .IsUnique();

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.RateToBase)
                      .IsRequired()
                      .HasPrecision(18, 6);
            });
        }

    }
}
