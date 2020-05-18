using System.Collections.Generic;
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
  public class Order : IEntity
  {
    /// <summary>
    /// The order's unique ID
    /// </summary>
    /// <value></value>
    [Required]
    public int ID { get; set; }
    /// <summary>
    /// The customer that made the order
    /// </summary>
    /// <value></value>
    [Required]
    public VendorAppUser User { get; set; }
    /// <summary>
    /// Collection of items that were made with this order
    /// </summary>
    /// <value>Generic collection list of OrderItem</value>
    public ICollection<OrderItem> OrderItems { get; set; }
    ///
    /// <summary>
    /// The timestamp when the purchase was made
    /// </summary>
    /// <value></value>
    [Required]
    // Assign get date within DBContext OnModelCreating
    public DateTime CreatedDate { get; set; }

    public Order()
    {
      // Set the time the order was made on creation
      this.CreatedDate = DateTime.UtcNow;
    }
  }
}