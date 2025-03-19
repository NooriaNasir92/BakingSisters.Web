using BakingSisters.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BakingSisters.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(8).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.WhatsAppNumber).HasMaxLength(50);
            entity.Property(e => e.StreetAddress).HasMaxLength(50);
            entity.Property(e => e.ZipCode).HasMaxLength(50);
            entity.Property(e => e.UserType);
            entity.Property(e => e.City).HasMaxLength(30);
        });
    }
}
          