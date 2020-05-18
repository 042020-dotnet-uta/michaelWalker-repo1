using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorApp.Models.Products;

using VendorApp.Data;

namespace VendorApp.Models.Locations
{
  /// <summary>
  /// A locations inventory of a particular product
  /// </summary>
  public class LocationInventory : IEntity
  {
    public int ID { get; set; }
    /// <summary>
    /// The prodcut's current location
    /// </summary>
    /// <value></value>
    [Required]
    public Location Location { get; set; }
    /// <summary>
    /// The product stocked at the current location
    /// </summary>
    /// <value></value>
    [Required]
    public Product Product { get; set; }
    /// <summary>
    /// The amount of a specific product at the location
    /// </summary>
    /// <value></value>
    [Required]
    public int Quanitity { get; set; }

    public override string ToString()
    {
      return $"Location: {Location.Name} Product: {Product.Name} Quantity: {Quanitity}";
    }
  }
}