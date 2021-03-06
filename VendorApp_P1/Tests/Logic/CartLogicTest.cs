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
//   public class CartLogicTest : IDisposable
//   {
//     private readonly P1ProtoDBContext ctx;

//     public CartLogicTest()
//     {
//       var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
//         .UseSqlite("Filename=Test.db")
//         .Options;

//       ctx = new P1ProtoDBContext(opts);

//       Seed();
//     }

//     // Seed the TestDB
//     private void Seed()
//     {
//       ctx.Database.EnsureDeleted();
//       ctx.Database.EnsureCreated();
//     }

//     [Fact]
//     public async void ItShouldFetchCartFromUser()
//     {
//       // * Arrange
//       EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);
//       // Mock user
//       string userId = "user-id";
//       VendorAppUser tempUser = new VendorAppUser
//       {
//         Id = userId
//       };

//       Cart newCart = new Cart();

//       // * Act
//       newCart.User = tempUser;
//       await cartRepo.Add(newCart);

//       // * Assert
//       Cart actualCart = await cartRepo.FindCartByUserId(userId);
//       Assert.NotNull(actualCart);
//     }

//     [Fact]
//     public async void ItShouldAddCartItemsForUser()
//     {
//       // * Arrange
//       EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);

//       // Make a mock user who will own the cart
//       string userId = "user-id";
//       VendorAppUser tempUser = new VendorAppUser
//       {
//         Id = userId
//       };
//       // Mock<EFCoreUserRepository> mockUser = new Mock<EFCoreUserRepository>();
//       // mockUser.Setup(u => u.Get(userId)).ReturnsAsync(tempUser);
//       // Make a cart
//       Cart newCart = new Cart
//       {
//         User = tempUser,
//         CartItems = new List<CartItem>()
//       };

//       // * Act
//       // Add cart to DbContext
//       await cartRepo.Add(newCart);

//       // Make some cart items

//       CartItem c1 = new CartItem
//       {
//         Cart = newCart,
//         ProductName = "Product1",
//         AmountPurchased = 2
//       };

//       CartItem c2 = new CartItem
//       {
//         Cart = newCart,
//         ProductName = "Product2",
//         AmountPurchased = 2
//       };
//       CartItem c3 = new CartItem
//       {
//         Cart = newCart,
//         ProductName = "Product3",
//         AmountPurchased = 4
//       };

//       // Add items to cart
//       await cartRepo.AddItemToCart(userId, "Product0", "Location0", 2);
//       await cartRepo.AddItemToCart(userId, "Product1", "Location1", 10);
//       await cartRepo.AddItemToCart(userId, "Product2", "Location2", 50);

//       // * Assert
//       // Retrive Cart back from context
//       Cart actualCart = await cartRepo.FindCartByUserId(tempUser.Id);

//       // Assert all values

//       Assert.Equal(userId, actualCart.User.Id);
//       // Check all cart items
//       List<CartItem> actualCartItems = (List<CartItem>)actualCart.CartItems;
//       Assert.Equal(3, actualCartItems.Count);

//       Assert.Equal("Product0", actualCartItems[0].ProductName);
//       Assert.Equal(2, actualCartItems[0].AmountPurchased);
//       Assert.Equal("Product2", actualCartItems[2].ProductName);
//       Assert.Equal(50, actualCartItems[2].AmountPurchased);
//     }

//     [Fact]
//     public async void ItShouldUpdateUsersNumCartItems()
//     {
//       //Given
//       EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       VendorAppUser newUser = new VendorAppUser();

//       //When
//       await userRepo.Add(newUser);


//       newUser = (await userRepo.GetAll()).ElementAt(0);

      

//       await cartRepo.AddItemToCart(newUser.Id, "product", "location", 1);

//       //Then
//       VendorAppUser actualUser = (await userRepo.GetAll()).ElementAt(0);
      
//       Assert.Equal(1, actualUser.NumCartItems);
//     }

//     [Fact]
//     public async void ItShouldClearUsersCart()
//     {
//       EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);
//       EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
//       VendorAppUser newUser = new VendorAppUser();

//       await userRepo.Add(newUser);

//       newUser = (await userRepo.GetAll()).ElementAt(0);

//       Cart newCart = new Cart
//       {
//         User = newUser
//       };

//       await cartRepo.Add(newCart);

//       List<Cart> fetched = await cartRepo.GetAll();

//       await cartRepo.Delete(fetched.ElementAt(0).ID);

//       Assert.Equal(0, (await cartRepo.GetAll()).Count());
//     }


//     public void Dispose() => ctx.Dispose();
//   }
// }