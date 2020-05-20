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

using VendorApp.Models.Locations;
using VendorApp.Models.Products;
using VendorApp.Models.Carts;
using VendorApp.Models.Users;
using VendorApp.Models.Orders;

namespace VendorApp.Tests
{
  public class LogicTest : IDisposable
  {
    private readonly P1ProtoDBContext ctx;
    private readonly VendorAppUser mockUser;

    private readonly Order mockOrder;
    private readonly OrderItem mockOrderItem;

    private readonly Location mockLocation;

    private readonly Product mockProduct;

    private readonly LocationInventory mockLocationInventory;

    public LogicTest()
    {
      var opts = new DbContextOptionsBuilder<P1ProtoDBContext>()
        .UseSqlite("Filename=Test.db")
        .Options;

      mockUser = new VendorAppUser
      {
        UserName = "username"
      };

      mockLocation = new Location
      {
        Name = "location"
      };

      mockProduct = new Product
      {
        Name = "product",
        Catagory = "catagory",
        FAClass = "icon"
      };

      mockLocationInventory = new LocationInventory
      {
        Product = mockProduct,
        Location = mockLocation,
        Quanitity = 1
      };

      mockOrder = new Order();

      mockOrderItem = new OrderItem
      {
        ProductName = "product",
        LocationName = "location",
        AmountPurchased = 1
      };

      ctx = new P1ProtoDBContext(opts);

      Seed();
    }

    private void Seed()
    {
      ctx.Database.EnsureDeleted();
      ctx.Database.EnsureCreated();
    }

    // * Location Tests

    [Fact]
    public async void ItShouldFindTheInventoryByLocationAndProductname()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);

      //When
      await locationRepo.AddLocationInventory(mockLocationInventory);

      //Then
      LocationInventory actualLocationInventory =
        await locationRepo.GetLocationInventoryByProductAndLocationName(mockLocation.Name, mockProduct.Name);
      Assert.Equal(1, actualLocationInventory.Quanitity);
    }

    [Fact]
    public async void ItShouldDecreaseInventory()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
      //When
      await locationRepo.AddLocationInventory(mockLocationInventory);

      await locationRepo.RemoveInventory(1, mockLocation.Name, mockProduct.Name);

      //Then
      LocationInventory actualLocationInventory =
          await locationRepo.GetLocationInventoryByProductAndLocationName(mockLocation.Name, mockProduct.Name);
      Assert.Equal(0, actualLocationInventory.Quanitity);
    }

    [Fact]
    public async void ItShouldRestockInventory()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
      //When
      await locationRepo.AddLocationInventory(mockLocationInventory);

      await locationRepo.RemoveInventory(-1, mockLocation.Name, mockProduct.Name);

      //Then
      LocationInventory actualLocationInventory =
          await locationRepo.GetLocationInventoryByProductAndLocationName(mockLocation.Name, mockProduct.Name);
      Assert.Equal(2, actualLocationInventory.Quanitity);
    }

    [Fact]
    public async void ItShouldRemoveLocationInventoryRecord()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);

      //When
      LocationInventory tempLocationInventory = await locationRepo.AddLocationInventory(mockLocationInventory);
      await locationRepo.RemoveLocationInventoryRecord(tempLocationInventory.ID);

      //Then

      Assert.Empty(await locationRepo.GetAllLocationInventoryRecords());
    }

    [Fact]
    public async void ItShouldCreateALocation()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
      Location newLocation = new Location
      {
        Name = "location"
      };
      //When
      await locationRepo.Add(newLocation);

      //Then
      Location actualLocation = (await locationRepo.GetAll()).ElementAt(0);
      Assert.Equal("location", actualLocation.Name);
    }

    [Fact]
    public async void ItShouldUpdateALocation()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
      Location newLocation = new Location
      {
        Name = "location"
      };
      //When
      await locationRepo.Add(newLocation);

      Location tempLocation = (await locationRepo.GetAll()).ElementAt(0);
      tempLocation.Name = "otherLocation";
      await locationRepo.Update(tempLocation);

      //Then
      Location actualLocation = (await locationRepo.GetAll()).ElementAt(0);
      Assert.Equal("otherLocation", actualLocation.Name);
    }

    [Fact]
    public async void ItShouldRemoveALocation()
    {
      //Given
      EFCoreLocationRepository locationRepo = new EFCoreLocationRepository(ctx);
      Location newLocation = new Location
      {
        Name = "location"
      };

      //When
      await locationRepo.Add(newLocation);
      Location tempLocation = (await locationRepo.GetAll()).ElementAt(0);
      await locationRepo.Delete(tempLocation.ID);

      //Then
      Assert.Equal(0, (await locationRepo.GetAll()).Count());
    }

    // * Cart Tests

    [Fact]
    public async void ItShouldFetchCartFromUser()
    {
      // * Arrange
      EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);
      // Mock user
      string userId = "user-id";
      VendorAppUser tempUser = new VendorAppUser
      {
        Id = userId
      };

      Cart newCart = new Cart();

      // * Act
      newCart.User = tempUser;
      await cartRepo.Add(newCart);

      // * Assert
      Cart actualCart = await cartRepo.FindCartByUserId(userId);
      Assert.NotNull(actualCart);
    }

    [Fact]
    public async void ItShouldAddCartItemsForUser()
    {
      // * Arrange
      EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);

      // Make a mock user who will own the cart
      string userId = "user-id";
      VendorAppUser tempUser = new VendorAppUser
      {
        Id = userId
      };
      // Mock<EFCoreUserRepository> mockUser = new Mock<EFCoreUserRepository>();
      // mockUser.Setup(u => u.Get(userId)).ReturnsAsync(tempUser);
      // Make a cart
      Cart newCart = new Cart
      {
        User = tempUser,
        CartItems = new List<CartItem>()
      };

      // * Act
      // Add cart to DbContext
      await cartRepo.Add(newCart);

      // Make some cart items

      CartItem c1 = new CartItem
      {
        Cart = newCart,
        ProductName = "Product1",
        AmountPurchased = 2
      };

      CartItem c2 = new CartItem
      {
        Cart = newCart,
        ProductName = "Product2",
        AmountPurchased = 2
      };
      CartItem c3 = new CartItem
      {
        Cart = newCart,
        ProductName = "Product3",
        AmountPurchased = 4
      };

      // Add items to cart
      await cartRepo.AddItemToCart(userId, "Product0", "Location0", 2);
      await cartRepo.AddItemToCart(userId, "Product1", "Location1", 10);
      await cartRepo.AddItemToCart(userId, "Product2", "Location2", 50);

      // * Assert
      // Retrive Cart back from context
      Cart actualCart = await cartRepo.FindCartByUserId(tempUser.Id);

      // Assert all values

      Assert.Equal(userId, actualCart.User.Id);
      // Check all cart items
      List<CartItem> actualCartItems = (List<CartItem>)actualCart.CartItems;
      Assert.Equal(3, actualCartItems.Count);

      Assert.Equal("Product0", actualCartItems[0].ProductName);
      Assert.Equal(2, actualCartItems[0].AmountPurchased);
      Assert.Equal("Product2", actualCartItems[2].ProductName);
      Assert.Equal(50, actualCartItems[2].AmountPurchased);
    }

    [Fact]
    public async void ItShouldUpdateUsersNumCartItems()
    {
      //Given
      EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      VendorAppUser newUser = new VendorAppUser();

      //When
      await userRepo.Add(newUser);


      newUser = (await userRepo.GetAll()).ElementAt(0);



      await cartRepo.AddItemToCart(newUser.Id, "product", "location", 1);

      //Then
      VendorAppUser actualUser = (await userRepo.GetAll()).ElementAt(0);

      Assert.Equal(1, actualUser.NumCartItems);
    }

    [Fact]
    public async void ItShouldClearUsersCart()
    {
      EFCoreCartRepository cartRepo = new EFCoreCartRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      VendorAppUser newUser = new VendorAppUser();

      await userRepo.Add(newUser);

      newUser = (await userRepo.GetAll()).ElementAt(0);

      Cart newCart = new Cart
      {
        User = newUser
      };

      await cartRepo.Add(newCart);

      List<Cart> fetched = await cartRepo.GetAll();

      await cartRepo.Delete(fetched.ElementAt(0).ID);

      Assert.Equal(0, (await cartRepo.GetAll()).Count());
    }

    // * Order Tests

    [Fact]
    public async void ItShouldCreateAOrder()
    {
      //Given
      EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      await userRepo.Add(mockUser);
      VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
      //When
      mockOrder.User = newUser;
      await orderRepo.Add(mockOrder);

      //Then
      Order actualOrder = (await orderRepo.GetAll()).ElementAt(0);
      Assert.Equal(1, actualOrder.ID);
    }

    [Fact]
    public async void ItShouldUpdateAOrder()
    {
      //Given
      EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      await userRepo.Add(mockUser);
      VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
      //When
      mockOrder.User = newUser;
      await orderRepo.Add(mockOrder);

      Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
      tempOrder.OrderItems = new List<OrderItem>{
        mockOrderItem
      };
      await orderRepo.Update(tempOrder);

      //Then
      Order actualOrder = (await orderRepo.GetAll()).ElementAt(0);
      Assert.Equal(1, actualOrder.OrderItems.Count());
    }

    [Fact]
    public async void ItShouldRemoveAOrder()
    {
      //Given
      EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      await userRepo.Add(mockUser);
      VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
      //When
      mockOrder.User = newUser;
      await orderRepo.Add(mockOrder);
      Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
      await orderRepo.Delete(tempOrder.ID);

      //Then
      Assert.Equal(0, (await orderRepo.GetAll()).Count());
    }

    [Fact]
    public async void ItShouldCreateAnOrderItem()
    {
      //Given
      EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      await userRepo.Add(mockUser);
      VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
      //When
      mockOrder.User = newUser;
      await orderRepo.Add(mockOrder);

      Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
      tempOrder.OrderItems = new List<OrderItem>{
        mockOrderItem
      };
      await orderRepo.Update(tempOrder);

      //Then
      OrderItem actualOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
      Assert.Equal("product", actualOrderItem.ProductName);
    }

    [Fact]
    public async void ItShouldUpdateAnOrderItem()
    {
      //Given
      EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      await userRepo.Add(mockUser);
      VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
      //When
      mockOrder.User = newUser;
      await orderRepo.Add(mockOrder);

      Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
      tempOrder.OrderItems = new List<OrderItem>{
        mockOrderItem
      };
      await orderRepo.Update(tempOrder);

      OrderItem tempOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
      tempOrderItem.ProductName = "New Product Name";
      await orderRepo.UpdateOrderItem(tempOrderItem);

      //Then
      OrderItem actualOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
      Assert.Equal("New Product Name", actualOrderItem.ProductName);
    }

    [Fact]
    public async void ItShouldRemoveAnOrderItem()
    {
      //Given
      EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(ctx);
      EFCoreUserRepository userRepo = new EFCoreUserRepository(ctx);
      await userRepo.Add(mockUser);
      VendorAppUser newUser = (await userRepo.GetAll()).ElementAt(0);
      //When
      mockOrder.User = newUser;
      await orderRepo.Add(mockOrder);

      Order tempOrder = (await orderRepo.GetAll()).ElementAt(0);
      tempOrder.OrderItems = new List<OrderItem>{
        mockOrderItem
      };
      await orderRepo.Update(tempOrder);
      OrderItem tempOrderItem = (await orderRepo.GetAllOrderItems()).ElementAt(0);
      await orderRepo.RemoveOrderItem(tempOrderItem.ID);

      //Then
      Assert.Equal(0, (await orderRepo.GetAllOrderItems()).Count());
    }


    [Fact]
    public async void ItShouldCreateAProduct()
    {
      //Given
      EFCoreProductRepository productRepo = new EFCoreProductRepository(ctx);
      //When
      await productRepo.Add(mockProduct);

      //Then
      Product actualProduct = (await productRepo.GetAll()).ElementAt(0);
      Assert.Equal("product", actualProduct.Name);
    }

    [Fact]
    public async void ItShouldUpdateAProduct()
    {
      //Given
      EFCoreProductRepository productRepo = new EFCoreProductRepository(ctx);
      //When
      await productRepo.Add(mockProduct);

      Product tempProduct = (await productRepo.GetAll()).ElementAt(0);
      tempProduct.Name = "otherProduct";
      await productRepo.Update(tempProduct);

      //Then
      Product actualProduct = (await productRepo.GetAll()).ElementAt(0);
      Assert.Equal("otherProduct", actualProduct.Name);
    }

    [Fact]
    public async void ItShouldRemoveAProduct()
    {
      //Given
      EFCoreProductRepository productRepo = new EFCoreProductRepository(ctx);

      //When
      await productRepo.Add(mockProduct);
      Product tempProduct = (await productRepo.GetAll()).ElementAt(0);
      await productRepo.Delete(tempProduct.ID);

      //Then
      Assert.Equal(0, (await productRepo.GetAll()).Count());
    }


    public void Dispose() => ctx.Dispose();


    // [Fact]
    // public async void ItShouldCreateALocation()
    // {
    // //Given

    // //When

    // //Then
    // }
  }
}