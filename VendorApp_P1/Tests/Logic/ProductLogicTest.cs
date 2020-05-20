// using System.Linq;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Xunit;
// using Moq;

// using Microsoft.EntityFrameworkCore;

// using VendorApp.Controllers;
// using VendorApp.Data;
// using VendorApp.Data.EFCore;

// using VendorApp.Models.Products;
// using VendorApp.Models.Carts;
// using VendorApp.Models.Users;

// namespace VendorApp.Tests
// {
//   public class ProductLogicTest: IDisposable
//   {
//     private readonly P1ProtoDBContext ctx;
//     private readonly Product mockProduct;

//     public ProductLogicTest()
//     {
//       var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
//         .UseSqlite("Filename=Test.db")
//         .Options;

//       mockProduct = new Product
//       {
//         Name = "product",
//         Catagory = "catagory",
//         FAClass = "icon"
//       };

//       ctx = new P1ProtoDBContext(opts);

//       Seed();
//     }

//     private void Seed()
//     {
//       ctx.Database.EnsureDeleted();
//       ctx.Database.EnsureCreated();
//     }

//     [Fact]
//     public async void ItShouldCreateAProduct()
//     {
//       //Given
//       EFCoreProductRepository productRepo = new EFCoreProductRepository(ctx);
//       //When
//       await productRepo.Add(mockProduct);

//       //Then
//       Product actualProduct = (await productRepo.GetAll()).ElementAt(0);
//       Assert.Equal("product", actualProduct.Name);
//     }

//     [Fact]
//     public async void ItShouldUpdateAProduct()
//     {
//       //Given
//       EFCoreProductRepository productRepo = new EFCoreProductRepository(ctx);
//       //When
//       await productRepo.Add(mockProduct);

//       Product tempProduct = (await productRepo.GetAll()).ElementAt(0);
//       tempProduct.Name = "otherProduct";
//       await productRepo.Update(tempProduct);

//       //Then
//       Product actualProduct = (await productRepo.GetAll()).ElementAt(0);
//       Assert.Equal("otherProduct", actualProduct.Name);
//     }

//     [Fact]
//     public async void ItShouldRemoveAProduct()
//     {
//       //Given
//       EFCoreProductRepository productRepo = new EFCoreProductRepository(ctx);

//       //When
//       await productRepo.Add(mockProduct);
//       Product tempProduct = (await productRepo.GetAll()).ElementAt(0);
//       await productRepo.Delete(tempProduct.ID);

//       //Then
//       Assert.Equal(0, (await productRepo.GetAll()).Count());
//     }

//     public void Dispose() => ctx.Dispose();


//     // [Fact]
//     // public async void ItShouldCreateAProduct()
//     // {
//     // //Given

//     // //When

//     // //Then
//     // }
//   }
// }