using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VendorApp.Models.Users;

using VendorApp.Data;

namespace VendorApp.Models.Orders
{
  /// <summary>
  /// The Customer Model
  /// The customers info will be stored when they place an order
  /// </summary>
  public class OrderItem : IEntity
  {
    // TODO: establish relationship from Cart to CartItem
    /// <summary>
    /// The order's unique ID
    /// </summary>
    /// <value></value>
    [Required]
    public int ID { get; set; }
    /// <summary>
    /// The order item's order record
    /// </summary>
    /// <value></value>
    public Order Order { get; set; }
    /// <summary>
    /// The product the customer purchased in the order
    /// </summary>
    /// <value></value>
    [Required]
    public string ProductName { get; set; }
    /// <summary>
    /// The location the item came from 
    /// </summary>
    /// <value></value>
    [Required]
    public string LocationName { get; set; }
    /// <summary>
    /// The amount of the product purchased by the customer
    /// </summary>
    /// <value></value>
    [Required]
    public int AmountPurchased { get; set; }
  }
}