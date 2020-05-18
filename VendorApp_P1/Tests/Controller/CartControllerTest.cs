using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;

using Microsoft.EntityFrameworkCore;

using VendorApp.Controllers;
using VendorApp.Data;
using VendorApp.Data.EFCore;

using VendorApp.Models.Products;
using VendorApp.Models.Carts;
using VendorApp.Models.Users;

namespace VendorApp.Tests
{
  public class CartControllerTest : IDisposable
  {
    private readonly P1ProtoDBContext ctx;

    public CartControllerTest()
    {
      var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
        .UseSqlite("Filename=Test.db")
        .Options;

      ctx = new P1ProtoDBContext(opts);

      Seed();
    }

    // Seed the TestDB
    private void Seed()
    {
      ctx.Database.EnsureDeleted();
      ctx.Database.EnsureCreated();
    }

    [Fact]
    public void ItShouldDoSomething()
    {
      int id = 12;
      string name = "MyProduct";
      Product product = new Product { ID = id, Name = name };
      Mock<IRepository<Product>> mock = new Mock<IRepository<Product>>();
      mock.Setup(p => p.Get(id)).ReturnsAsync(product);

      // ProductController productController = new ProductController(mock.Object);
      // var actual = productController.Details(id);
      // Assert.Same(product, actual);
    }


    public void Dispose() => ctx.Dispose();
  }
}