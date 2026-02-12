using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarRent.Models;
using Microsoft.AspNetCore.Identity;
namespace CarRent.DataBase;
public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    // public DbSet<User> Users {get; set;}
    public DbSet<Car> Cars {get; set;} 
    public DbSet<Cart> Carts {get; set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Car>()
            .HasOne(c => c.User)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.IdUser);
        builder.Entity<IdentityRole>().HasData(
            new { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN" },
            new { Id = "3d5f285f-4c1f-557g-97bg-594e67ge8321", Name = "User", NormalizedName = "USER" }
        );
        builder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.IdUser);
        builder.Entity<Car>()
            .HasOne(c => c.Cart)
            .WithMany(c => c.Cars)
            .HasForeignKey(c => c.IdCart);
    }
}