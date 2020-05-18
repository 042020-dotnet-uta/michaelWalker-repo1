using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

using VendorApp.Data;
using VendorApp.Models.Orders;
using VendorApp.Models.Carts;

namespace VendorApp.Models.Users
{
  /// <summary>
  /// The Customer Model
  /// The customers info will be stored when they place an order
  /// </summary>
  public class VendorAppUser : IdentityUser, IEntity
  {
    // /// <summary>
    // /// Customer's unique ID
    // /// </summary>
    // /// <value></value>
    // public override string Id { get; set; }
    // /// <summary>
    // /// Customer's registered username
    // /// </summary>
    // /// <value></value>
    // [Required]
    // [MaxLength(50)]
    // public string Username { get; set; }
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <value></value>
    // [Required]
    // public string Password { get; set; }
    // /// <summary>
    // /// Customer's registered email
    // /// </summary>
    // /// <value></value>
    // [Required]
    // [MaxLength(50)]
    // public override string Email { get; set; }
    /// <summary>
    /// A list of the customer's order history
    /// </summary>
    /// <value></value>
    public ICollection<Order> OrderHistory { get; set; }
    /// <summary>
    /// The cart that the user can store items into before
    /// purchase
    /// </summary>
    /// <value></value>
    public Cart Cart { get; set; }
    /// <summary>
    /// The number of items the user currently has in their cart
    /// </summary>
    /// <value></value>
    public int NumCartItems { get; set; }

    public VendorAppUser()
    {
      NumCartItems = 0;
    }
  }
}