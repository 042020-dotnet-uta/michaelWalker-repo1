using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;


using VendorApp.Data;

namespace VendorApp.Models.Users
{
  /// <summary>
  /// The Customer Model
  /// The customers info will be stored when they place an order
  /// </summary>
  public class Role : IdentityUserRole<string>, IEntity
  {
    /// <summary>
    /// Role's unique ID
    /// </summary>
    /// <value></value>
    public int ID { get; set; }
    /// <summary>
    /// Role name that will be used to determine a user's
    /// permissions in the application
    /// </summary>
    /// <value></value>
    public string RoleName { get; set; }
  }
}