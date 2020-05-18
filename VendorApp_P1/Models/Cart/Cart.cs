using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using VendorApp.Data;
using VendorApp.Models.Users;

namespace VendorApp.Models.Carts
{
  /// <summary>
  /// The product that will be stored inside each location
  /// </summary>
  public class Cart : IEntity
  {
    /// <summary>
    /// Unique Identifier
    /// </summary>
    /// <value></value>
    public int ID { get; set; }
    // * Establish Foreign Key in DbContext onModelBuild
    public string UserId { get; set; }
    /// <summary>
    /// The user that this cart belongs to
    /// </summary>
    /// <value></value>
    [Required]
    public VendorAppUser User { get; set; }
    /// <summary>
    /// The items that belong to the cart
    /// </summary>
    public ICollection<CartItem> CartItems { get; set; }

    public override string ToString()
    {
      string cartItems = "\nCartItems:\n";
      int tempCount = 0;

      foreach (var cartItem in CartItems)
      {
        tempCount++;
        cartItems += $"Product: {cartItem.ProductName} Location: {cartItem.LocationName} Quantity: {cartItem.AmountPurchased}";
      }

      if (tempCount == 0)
      {
        cartItems = "There are no cart items";
      }

      return $"Cart for {User.UserName} {cartItems}";
    }
  }
}