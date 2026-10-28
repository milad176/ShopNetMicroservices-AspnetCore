using Identity.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<ApplicationUser>();

        user.HasKey(x => x.Id);

        user.HasIndex(x => new { x.TenantId, x.ObjectId })
            .IsUnique();

        user.Property(x => x.TenantId).HasMaxLength(100).IsRequired();
        user.Property(x => x.ObjectId).HasMaxLength(100).IsRequired();
        user.Property(x => x.Issuer).HasMaxLength(500).IsRequired();
        user.Property(x => x.Subject).HasMaxLength(200).IsRequired();
    }
}