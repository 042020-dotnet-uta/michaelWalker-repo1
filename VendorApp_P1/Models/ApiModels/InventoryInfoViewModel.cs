using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VendorApp.Models.Products;
using VendorApp.Models.Locations;

namespace VendorApp.Models.ApiModels
{
  /// <summary>
  /// This model will provide a list of all known locations and a specified target location's inventory.
  /// This scheme will be used to send api responses that contains the related data.
  /// </summary>
  public class InventoryInfo
  {
    /// <summary>
    /// List of all known locations
    /// </summary>
    public ICollection<Location> Locations { get; set; }
    /// <summary>
    /// A specified location's inventory records which contains information
    /// on which product the location has and the quantity of that product in
    /// it's posession.
    /// </summary>
    /// <value></value>
    public ICollection<LocationInventory> TargetInventory { get; set; }
    /// <summary>
    /// List of known product catagories
    /// </summary>
    public ICollection<Catagory> Catagories { get; set; }
  }
}