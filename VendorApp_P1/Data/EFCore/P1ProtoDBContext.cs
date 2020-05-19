using Microsoft.EntityFrameworkCore;

using VendorApp.Models.Orders;
using VendorApp.Models.Users;
using VendorApp.Models.Locations;
using VendorApp.Models.Products;
using VendorApp.Models.Carts;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



namespace VendorApp.Data
{
  public class P1ProtoDBContext : IdentityDbContext<VendorAppUser>
  {
    /// <summary>
    /// DBSet of Locations
    /// </summary>
    /// <value></value>
    public DbSet<Location> Locations { get; set; }
    /// <summary>
    /// DBSet of Orders 
    /// </summary>
    /// <value></value>
    public DbSet<Order> Orders { get; set; }
    /// <summary>
    /// DBSet of Orders Items
    /// </summary>
    /// <value></value>
    public DbSet<OrderItem> OrderItems { get; set; }
    /// <summary>
    /// DBSet of ProductCatagories 
    /// </summary>
    /// <value></value>
    public DbSet<ProductCatagory> ProductCatagories { get; set; }
    /// <summary>
    /// DBSet of Products 
    /// </summary>
    /// <value></value>
    public DbSet<Product> Products { get; set; }
    /// <summary>
    /// DBSet of Catagories 
    /// </summary>
    /// <value></value>
    public DbSet<Catagory> Catagories { get; set; }
    /// <summary>
    /// DBSet of LocationInventoryRecords 
    /// </summary>
    /// <value></value>
    public DbSet<Cart> Carts { get; set; }
    /// <summary>
    /// DBSet of LocationInventoryRecords 
    /// </summary>
    /// <value></value>
    public DbSet<CartItem> CartItems { get; set; }
    /// <summary>
    /// DBSet of LocationInventoryRecords 
    /// </summary>
    /// <value></value>
    public DbSet<LocationInventory> LocationInventoryRecords { get; set; }

    public P1ProtoDBContext(DbContextOptions<P1ProtoDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // * Establish User has one Cart relation
      builder.Entity<VendorAppUser>()
        .HasOne(u => u.Cart)
        .WithOne(c => c.User)
        .HasForeignKey<Cart>(c => c.UserId);
    }

  }
}