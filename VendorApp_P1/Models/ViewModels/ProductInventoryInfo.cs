using System.Collections.Generic;

using VendorApp.Models.Locations;

namespace VendorApp.Models.ViewModels
{
  /// <summary>
  /// This will provide a designated LocationInventory related to a specifc product that will be
  /// presented into the view when the user wants to view a product to add to their cart.
  /// This model will also provide a list of all the locations that has this particular product
  /// in their inventory along with the quantiy of that product available.
  /// </summary>
  public class ProductInventoryInfo
  {
    /// <summary>
    /// LocationInvetory info providing the products location and the quanttity
    /// of this product the location currently has
    /// </summary>
    /// <value></value>
    public LocationInventory TargetLocationInventory { get; set; }
    /// <summary>
    /// Collection of LocationInventory related to a specifc product
    /// </summary>
    /// <value></value>
    public ICollection<LocationInventory> ProductRelatedLocationInventoryRecords { get; set; }

    public override string ToString()
    {
      return $" Location{TargetLocationInventory.Location.Name}";
    }
  }
}