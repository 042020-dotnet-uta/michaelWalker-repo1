using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using VendorApp.Models.Locations;
using VendorApp.Models.Orders;

namespace VendorApp.Models.ViewModels
{
  public class LocationOrders
  {
      /// <summary>
      /// Name of the location
      /// </summary>
      public string LocationName { get; set; }
      /// <summary>
      /// List of items that were recorded to be purchased
      /// from this location
      /// </summary>
      public ICollection<OrderItem> OrderItems { get; set; }
  }
}