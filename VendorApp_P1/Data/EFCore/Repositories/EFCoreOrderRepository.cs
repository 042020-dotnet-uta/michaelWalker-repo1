using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using VendorApp.Models.Users;
using VendorApp.Models.Orders;
using VendorApp.Models.Carts;
using VendorApp.Models.Locations;

namespace VendorApp.Data.EFCore
{
  public class EFCoreOrderRepository : EFCoreRepository<Order, P1ProtoDBContext>
  {
    private readonly P1ProtoDBContext context;
    public EFCoreOrderRepository(P1ProtoDBContext ctx) : base(ctx)
    {
      context = ctx;
    }

    // Now we can

    /// <summary>
    /// Fetch a list of orders made by a specified user from the DB
    /// </summary>
    /// <param name="userId">Unique id of a user</param>
    /// <returns>An ICollection of Orders</returns>
    public async Task<ICollection<Order>> GetOrdersByUserId(string userId)
    {
      return await context.Orders
        .Include(o => o.OrderItems)
        .Where(o => o.User.Id == userId)
        .OrderByDescending(o => o.CreatedDate)
        .ToListAsync();
    }

    /// <summary>
    /// Fetch a list of orders that were made by the specified location sorted by date
    /// </summary>
    /// <param name="locationName"></param>
    /// <returns></returns>
    public async Task<ICollection<OrderItem>> GetOrderItemsByLocationName(string locationName)
    {
      // Get all order items that match the location name
      return await context.OrderItems
        .Include(oI => oI.Order)
        .ThenInclude(o => o.User)
        .Where(oI => oI.LocationName == locationName)
        .OrderByDescending(oI => oI.Order.CreatedDate)
        .ToListAsync();
    }

    // TODO: refine this set of docs
    /// <summary>
    /// Records a list of order
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<Order> RecordOrder(string userId)
    {
      // Create new order record and begin filling in the needed data from the user's cart
      EFCoreCartRepository cartRepo = new EFCoreCartRepository(context);
      Cart userCart = await cartRepo.FindCartByUserId(userId);

      Order newUserOrder = new Order
      {
        User = userCart.User,
        OrderItems = new List<OrderItem>()
      };

      // ? If the cart is empty return The empty cart and
      // ? without saving any data to DB
      if (userCart.CartItems.Count() == 0)
      {
        return newUserOrder;
      }

      // Start filling in The OrderItems list with the cart items
      foreach (CartItem cartItem in userCart.CartItems)
      {
        newUserOrder.OrderItems.Add(new OrderItem
        {
          ProductName = cartItem.ProductName,
          LocationName = cartItem.LocationName,
          AmountPurchased = cartItem.AmountPurchased
        });

        // TODO: REALLY bad to update if one of these fails
        //       Find alternative implementation
        // LocationInventory updateLocatInventory = 
        //   await new EFCoreLocationRepository(context)
        //   .RemoveInventory( cartItem.AmountPurchased,cartItem.LocationName,cartItem.ProductName);
      }

      // Delete the cart - order has been sucessfully documented
      await cartRepo.Delete(userCart.ID);



      // All required data has been added, we can now add to DB and return the newly
      // created order
      return await this.Add(newUserOrder);
    }


    /// <summary>
    /// Returns all known order items
    /// </summary>
    /// <returns></returns>
    public async Task<ICollection<OrderItem>> GetAllOrderItems(){
      return await context.OrderItems.ToListAsync();
    }

    /// <summary>
    /// Creates an order item
    /// </summary>
    /// <returns></returns>
    public async Task<OrderItem> AddOrderItem(OrderItem orderItem) {
      context.OrderItems.Add(orderItem);
      await context.SaveChangesAsync();
      return orderItem;
    }

    /// <summary>
    /// Updates an order item
    /// </summary>
    /// <returns></returns>
    public async Task<OrderItem> UpdateOrderItem(OrderItem orderItem) {
      context.OrderItems.Update(orderItem);
      await context.SaveChangesAsync();
      return orderItem;
    }

    /// <summary>
    /// Updates an order item
    /// </summary>
    /// <returns></returns>
    public async Task<OrderItem> RemoveOrderItem(int id) {
      OrderItem orderItem = await context.OrderItems.FindAsync(id);

      if (orderItem == null)
      {
        // TODO: Log that no Entity was found to delete here or in Repo
        return orderItem;
      }

      context.OrderItems.Remove(orderItem);

      await context.SaveChangesAsync();

      return orderItem;
    }
  }
}