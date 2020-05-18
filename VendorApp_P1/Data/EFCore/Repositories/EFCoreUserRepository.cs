using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


using VendorApp.Models.Users;
using VendorApp.Models.Carts;
using VendorApp.Models;
using VendorApp.Data.EFCore;

namespace VendorApp.Data.EFCore
{
  public class EFCoreUserRepository : EFCoreRepository<VendorAppUser, P1ProtoDBContext>
  {
    private readonly P1ProtoDBContext context;

    public EFCoreUserRepository(P1ProtoDBContext context) : base(context)
    {
      this.context = context;
    }

    public async Task<VendorAppUser> Get(string id)
    {
      return await context.Set<VendorAppUser>().FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Updates the amount of cart items a user has
    /// </summary>
    /// <param name="userId">User's unique id</param>
    /// <param name="numCartItems">The amount of cart items the will overwrite the previous value</param>
    /// <returns>The new amount of NumCartItems</returns>
    public async Task<int> UpdateNumCartItems(string userId, int numCartItems)
    {
      VendorAppUser user = await context.Set<VendorAppUser>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
      user.NumCartItems = numCartItems;
      await context.SaveChangesAsync();
      return numCartItems;
    }

    public async Task<Cart> AddCartToUser(string userId, Cart cart)
    {
      VendorAppUser user = await Get(userId);

      user.Cart = cart;
      await context.SaveChangesAsync();

      return user.Cart;
    }

    // Now we can

    // TODO: Make a new role

    // TODO: Assign role to user

    // TODO: Have user make a purchase
    // public async Task<Order> RegisterOrderForUser(VendorAppUser user, string locationName, string productName, int quantityPurchased)
    // {
    //   EFCoreOrderRepository orderRepo = new EFCoreOrderRepository(context);
    //   Order newOrder = await orderRepo.Add(new Order
    //   {
    //     User = user,
    //     ProductName = productName,
    //     LocationName = locationName,
    //     AmountPurchased = quantityPurchased
    //   });

    //   return newOrder;
    // }


    public async Task<Cart> AddItemToCart(int userId, string productName, int quantity)
    {
      // Get instance of user from id

      // Check if user already has cart for us to use
      // If user has no cart, create one for them



      return null;
    }

  }
}