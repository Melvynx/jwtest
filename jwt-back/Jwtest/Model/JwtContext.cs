using Microsoft.EntityFrameworkCore;

namespace Jwtest;

public class JwtContext : DbContext
{
    public JwtContext(DbContextOptions<JwtContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // add user on hasData params
        var adminUser = new User
        {
            Id = 1,
            FirstName = "Admin",
            LastName = "Admin",
            Username = "admin",
            Password = "admin"
        };

        modelBuilder.Entity<User>().HasData(adminUser);
        base.OnModelCreating(modelBuilder);
    }
}