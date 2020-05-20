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

// using VendorApp.Models.Locations;
// using VendorApp.Models.Products;
// using VendorApp.Models.Users;

// namespace VendorApp.Tests
// {
//   public class LocationLogicTest: IDisposable
//   {
//     private readonly P1ProtoDBContext ctx;
//     private readonly Location mockLocation;

//     private readonly Product mockProduct;

//     private readonly LocationInventory mockLocationInventory;

//     public LocationLogicTest()
//     {
//       var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
//         .UseSqlite("Filename=Test.db")
//         .Options;

//       mockLocation = new Location
//       {
//         Name = "location"
//       };

//       mockProduct = new Product
//       {
//         Name = "product",
//         Catagory = "catagory",
//         FAClass = "icon"
//       };

//       mockLocationInventory = new LocationInventory
//       {
//         Product = mockProduct,
//         Location = mockLocation,
//         Quanitity = 1
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
//     public async void ItShouldFindTheInventoryByLocationAndProductname()
//     {
//       //Given
//       EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);

//       //When
//       await locationRepo.AddLocationInventory(mockLocationInventory);

//       //Then
//       LocationInventory actualLocationInventory =
//         await locationRepo.GetLocationInventoryByProductAndLocationName(mockLocation.Name, mockProduct.Name);
//       Assert.Equal(1, actualLocationInventory.Quanitity);
//     }

//     [Fact]
//     public async void ItShouldDecreaseInventory()
//     {
//       //Given
//       EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
//       //When
//       await locationRepo.AddLocationInventory(mockLocationInventory);

//       await locationRepo.RemoveInventory(1, mockLocation.Name, mockProduct.Name);

//       //Then
//       LocationInventory actualLocationInventory =
//           await locationRepo.GetLocationInventoryByProductAndLocationName(mockLocation.Name, mockProduct.Name);
//       Assert.Equal(0, actualLocationInventory.Quanitity);
//     }

//     [Fact]
//     public async void ItShouldRestockInventory()
//     {
//       //Given
//       EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
//       //When
//       await locationRepo.AddLocationInventory(mockLocationInventory);

//       await locationRepo.RemoveInventory(-1, mockLocation.Name, mockProduct.Name);

//       //Then
//       LocationInventory actualLocationInventory =
//           await locationRepo.GetLocationInventoryByProductAndLocationName(mockLocation.Name, mockProduct.Name);
//       Assert.Equal(2, actualLocationInventory.Quanitity);
//     }

//     [Fact]
//     public async void ItShouldRemoveLocationInventoryRecord()
//     {
//     //Given
//     EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);

//     //When
//     LocationInventory tempLocationInventory = await locationRepo.AddLocationInventory(mockLocationInventory);
//     await locationRepo.RemoveLocationInventoryRecord(tempLocationInventory.ID);
    
//     //Then

//     Assert.Empty(await locationRepo.GetAllLocationInventoryRecords());
//     }

//     [Fact]
//     public async void ItShouldCreateALocation()
//     {
//       //Given
//       EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
//       Location newLocation = new Location
//       {
//         Name = "location"
//       };
//       //When
//       await locationRepo.Add(newLocation);

//       //Then
//       Location actualLocation = (await locationRepo.GetAll()).ElementAt(0);
//       Assert.Equal("location", actualLocation.Name);
//     }

//     [Fact]
//     public async void ItShouldUpdateALocation()
//     {
//       //Given
//       EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
//       Location newLocation = new Location
//       {
//         Name = "location"
//       };
//       //When
//       await locationRepo.Add(newLocation);

//       Location tempLocation = (await locationRepo.GetAll()).ElementAt(0);
//       tempLocation.Name = "otherLocation";
//       await locationRepo.Update(tempLocation);

//       //Then
//       Location actualLocation = (await locationRepo.GetAll()).ElementAt(0);
//       Assert.Equal("otherLocation", actualLocation.Name);
//     }

//     [Fact]
//     public async void ItShouldRemoveALocation()
//     {
//       //Given
//       EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
//       Location newLocation = new Location
//       {
//         Name = "location"
//       };

//       //When
//       await locationRepo.Add(newLocation);
//       Location tempLocation = (await locationRepo.GetAll()).ElementAt(0);
//       await locationRepo.Delete(tempLocation.ID);

//       //Then
//       Assert.Equal(0, (await locationRepo.GetAll()).Count());
//     }

//     public void Dispose() => ctx.Dispose();

//     // [Fact]
//     // public async void ItShouldCreateALocation()
//     // {
//     // //Given

//     // //When

//     // //Then
//     // }
//   }
// }