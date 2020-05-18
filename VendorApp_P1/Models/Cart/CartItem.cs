using System.ComponentModel.DataAnnotations;

using VendorApp.Data;

namespace VendorApp.Models.Carts
{
  /// <summary>
  /// The product that will be stored inside each location
  /// </summary>
  public class CartItem : IEntity
  {
    /// <summary>
    /// Unique Identifier
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Reference to the shopping cart this cart item belongs to
    /// </summary>
    [Required]
    public Cart Cart { get; set; }
    /// <summary>
    /// The location the item came from 
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(50)]
    public string LocationName { get; set; }
    /// <summary>
    /// Name of the Item in the cart 
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ProductName { get; set; }
    /// <summary>
    /// The amount of the item in the cart
    /// </summary>
    [Required]
    public int AmountPurchased { get; set; }
  }
}