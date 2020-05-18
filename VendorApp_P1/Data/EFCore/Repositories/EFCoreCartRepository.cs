using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using VendorApp.Models.Users;
using VendorApp.Models.Carts;
using VendorApp.Models.Locations;
using VendorApp.Models.Orders;

namespace VendorApp.Data.EFCore
{
  public class EFCoreCartRepository : EFCoreRepository<Cart, P1ProtoDBContext>
  {
    private readonly P1ProtoDBContext context;

    public EFCoreCartRepository(P1ProtoDBContext context) : base(context)
    {
      this.context = context;
    }

    // TODO: add docs
    public async Task<Cart> GetWithUser(int cartId)
    {
      return await context.Carts.AsNoTracking().Include(c => c.User).FirstOrDefaultAsync(c => c.ID == cartId);
    }

    /// <summary>
    /// Finds the cart that belings to a specific user.  Returns null 
    /// if no cart was found for user.
    /// </summary>
    /// <param name="userId">Qunique id of the user</param>
    /// <returns>
    /// The User's Cart or null if none was found.  The cart will include
    /// the User the cart belongs to and it's CartItems
    /// </returns>
    public async Task<Cart> FindCartByUserId(string userId)
    {
      return await context.Carts
        .Include(c => c.User)
        .Include(c => c.CartItems)
        .FirstOrDefaultAsync(c => c.User.Id == userId);
    }

    /// <summary>
    ///  Adds an item to a user's cart.  Creates a cart record if one hasn't been added for the 
    /// user. Returns null if the item was not sucessfully added to the cart.
    /// </summary>
    /// <param name="userId">Id of the user of the cart</param>
    /// <param name="productName">Name of the product being added</param>
    /// <param name="quantity">The amount of the product being added</param>
    /// <returns></returns>
    public async Task<Cart> AddItemToCart(string userId, string productName, string locationName, int quantity)
    {
      // Get instance of user from id
      EFCoreUserRepository userRepo = new EFCoreUserRepository(context);
      VendorAppUser user = await userRepo.Get(userId);
      Cart userCart;

      // If user doesn't exist stop process and return null
      if (user == null)
      {
        return null;
      }

      // Check if user already has cart for us to use
      // If user has no cart, create one for them
      userCart = await FindCartByUserId(userId);

      if (userCart == null)
      {
        // userCart = new Cart
        // {
        //   User = user
        // };
        user.Cart = new Cart();
      }

      // add item to cart
      CartItem newItem = new CartItem
      {
        Cart = user.Cart,
        ProductName = productName,
        LocationName = locationName,
        AmountPurchased = quantity
      };
      context.CartItems.Add(newItem);

      // Update the location inventory and the amount of cart items a user has
      LocationInventory updateLocatInventory =
          await new EFCoreLocationRepository(context)
          .RemoveInventory(newItem.AmountPurchased, newItem.LocationName, newItem.ProductName);
      // TODO: Update location inventory without using repo
      user.NumCartItems++;

      // save changes
      context.SaveChanges();


      return userCart;
    }

    // TODO: add docs
    public async Task<CartItem> RemoveItemFromCart(int cartId){
      // Find the cart item with it's cart and the cart's user
      CartItem cartItemToRemove = await context.CartItems
        .Include(cI => cI.Cart)
        .ThenInclude(c => c.User)
        .FirstOrDefaultAsync(cI => cI.ID == cartId);
      // Retreive location to restock it's inventory upon deletion
      LocationInventory locationInventoryToRestock = 
        await context.LocationInventoryRecords
        .FirstOrDefaultAsync(lIR => 
          lIR.Product.Name == cartItemToRemove.ProductName 
          && lIR.Location.Name == cartItemToRemove.LocationName);
      // Restock the inventory and update user's number of cart items
      locationInventoryToRestock.Quanitity += cartItemToRemove.AmountPurchased;
      cartItemToRemove.Cart.User.NumCartItems--;

      // Remove cart item
      context.CartItems.Remove(cartItemToRemove);

      // Save changes
      await context.SaveChangesAsync();

      return cartItemToRemove;
    }

    // TODO: add docs
    // public async Task<Cart> RegisterPurchase(string userId)
    // {
      
    //   Cart userCart = await FindCartByUserId(userId);
    //   EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(context);

    //   await orderRepo.RecordOrder(userId);

    //   return userCart;
    // }

    // TODO: add docs
    public async Task<Cart> RegisterPurchase(int cartId){
      // Retreive cart by id
      Cart cart = 
        await context.Carts.Include(c => c.User).Include(c => c.CartItems)
        .FirstOrDefaultAsync(c => c.ID == cartId);
      VendorAppUser cartUser = cart.User;

      // Create a blank order record for the user with an empty set of order items
      Order newOrder = new Order{
        User = cartUser,
        OrderItems = new List<OrderItem>()
      };

      // store the cart items list to the order items list
      foreach(var cartItem in cart.CartItems){
        newOrder.OrderItems.Add(new OrderItem{
          ProductName = cartItem.ProductName,
          LocationName = cartItem.LocationName,
          AmountPurchased = cartItem.AmountPurchased
        });
      }

      // Remove the cart and reset users number of cart tiems
      cartUser.NumCartItems = 0;
      context.Carts.Remove(cart);

      // Add order to db
      context.Orders.Add(newOrder);


      // save the changes
      await context.SaveChangesAsync();

      return cart;
    }

    /// <summary>
    /// Searches the DB for a the inventory of a location based on the location's name and the product
    /// the locatiion has in inventory
    /// </summary>
    /// <param name="locationName">Name of the location</param>
    /// <param name="productName">Name of the product</param>
    /// <returns>The location inventory model</returns>
    public async Task<LocationInventory> GetLocationInventoryByProductAndLocationName(string locationName, string productName)
    {
      return await new EFCoreLocationRepository(context).GetLocationInventoryByProductAndLocationName(locationName, productName);
    }

  }

}