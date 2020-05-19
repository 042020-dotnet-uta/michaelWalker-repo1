using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using VendorApp.Models.Users;
using VendorApp.Models.Orders;

namespace VendorApp.Models.ViewModels
{
    public class UserOrders
    {
      /// <summary>
      /// User who the list of orders belong to
      /// </summary>
      [Required]
      public string UserName { get; set; }

      /// <summary>
      /// List of the User's orders 
      /// </summary>
      [Required]
      public ICollection<Order> Orders { get; set; }
    }
}