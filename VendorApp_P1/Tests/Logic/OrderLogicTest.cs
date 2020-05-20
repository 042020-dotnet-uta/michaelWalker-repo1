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

// using VendorApp.Models.Orders;
// using VendorApp.Models.Carts;
// using VendorApp.Models.Users;

// namespace VendorApp.Tests
// {
//   public class OrderLogicTest: IDisposable
//   {
//     private readonly P1ProtoDBContext ctx;
//     private readonly Order mockOrder;
//     private readonly OrderItem mockOrderItem;
//     private readonly VendorAppUser mockUser;

//     public OrderLogicTest()
//     {
//       var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
//         .UseSqlite("Filename=Test.db")
//         .Options;

//       ctx = new P1ProtoDBContext(opts);

//       mockOrder = new Order();

//       mockOrderItem = new OrderItem
//       {
//         ProductName = "product",
//         LocationName = "location",
//         AmountPurchased = 1
//       };

//       mockUser = new VendorAppUser
//       {
//         UserName = "username"
//       };

//       Seed();
//     }

//     private void Seed()
//     {
//       ctx.Database.EnsureDeleted();
//       ctx.Database.EnsureCreated();
//     }

//     [Fact]
//     public async void ItShouldCreateAOrder()
//     {
//       //Given
//       EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       await userRepo.Add(mockUser);
//       VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
//       //When
//       mockOrder.User = newUser;
//       await orderRepo.Add(mockOrder);

//       //Then
//       Order actualOrder = (await orderRepo.GetAll()).ElementAt(0);
//       Assert.Equal(1, actualOrder.ID);
//     }

//     [Fact]
//     public async void ItShouldUpdateAOrder()
//     {
//       //Given
//       EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       await userRepo.Add(mockUser);
//       VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
//       //When
//       mockOrder.User = newUser;
//       await orderRepo.Add(mockOrder);

//       Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
//       tempOrder.OrderItems = new List<OrderItem>{
//         mockOrderItem
//       };
//       await orderRepo.Update(tempOrder);

//       //Then
//       Order actualOrder = (await orderRepo.GetAll()).ElementAt(0);
//       Assert.Equal(1, actualOrder.OrderItems.Count());
//     }

//     [Fact]
//     public async void ItShouldRemoveAOrder()
//     {
//       //Given
//       EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       await userRepo.Add(mockUser);
//       VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
//       //When
//       mockOrder.User = newUser;
//       await orderRepo.Add(mockOrder);
//       Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
//       await orderRepo.Delete(tempOrder.ID);

//       //Then
//       Assert.Equal(0, (await orderRepo.GetAll()).Count());
//     }

//     [Fact]
//     public async void ItShouldCreateAnOrderItem()
//     {
//       //Given
//       EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       await userRepo.Add(mockUser);
//       VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
//       //When
//       mockOrder.User = newUser;
//       await orderRepo.Add(mockOrder);

//       Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
//       tempOrder.OrderItems = new List<OrderItem>{
//         mockOrderItem
//       };
//       await orderRepo.Update(tempOrder);

//       //Then
//       OrderItem actualOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
//       Assert.Equal("product", actualOrderItem.ProductName);
//     }

//     [Fact]
//     public async void ItShouldUpdateAnOrderItem()
//     {
//       //Given
//       EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       await userRepo.Add(mockUser);
//       VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
//       //When
//       mockOrder.User = newUser;
//       await orderRepo.Add(mockOrder);

//       Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
//       tempOrder.OrderItems = new List<OrderItem>{
//         mockOrderItem
//       };
//       await orderRepo.Update(tempOrder);

//       OrderItem tempOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
//       tempOrderItem.ProductName = "New Product Name";
//       await orderRepo.UpdateOrderItem(tempOrderItem);

//       //Then
//       OrderItem actualOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
//       Assert.Equal("New Product Name", actualOrderItem.ProductName);
//     }

//     [Fact]
//     public async void ItShouldRemoveAnOrderItem()
//     {
//       //Given
//       EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       await userRepo.Add(mockUser);
//       VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
//       //When
//       mockOrder.User = newUser;
//       await orderRepo.Add(mockOrder);

//       Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
//       tempOrder.OrderItems = new List<OrderItem>{
//         mockOrderItem
//       };
//       await orderRepo.Update(tempOrder);
//       OrderItem tempOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
//       await orderRepo.RemoveOrderItem(tempOrderItem.ID);

//       //Then
//       Assert.Equal(0, (await orderRepo.GetAllOrderItems()).Count());
//     }


//     public void Dispose() => ctx.Dispose();

//     // [Fact]
//     // public async void ItShouldCreateAOrderItem()
//     // {
//     // //Given

//     // //When

//     // //Then
//     // }
//   }
// }