using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jwtest;

public class JwtContext : IdentityDbContext
{
    public JwtContext(DbContextOptions<JwtContext> options) : base(options)
    {
    }

    // public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}