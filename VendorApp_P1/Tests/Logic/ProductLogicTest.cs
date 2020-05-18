using System;
using System.Collections.Generic;
using Xunit;
using Moq;

using Microsoft.EntityFrameworkCore;


using VendorApp.Data;
using VendorApp.Data.EFCore;

using VendorApp.Models.Products;
using VendorApp.Models.Locations;


namespace VendorApp.Tests
{
  class ProductLogicTest
  {
    private readonly P1ProtoDBContext ctx;

    public ProductLogicTest()
    {
      var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
        .UseSqlite("Filename=Test.db")
        .Options;

      ctx = new P1ProtoDBContext(opts);

      Seed();
    }

    private void Seed()
    {
      ctx.Database.EnsureDeleted();
      ctx.Database.EnsureCreated();
    }

    // TODO: make test for retrieving product inventory by location id
  }
}