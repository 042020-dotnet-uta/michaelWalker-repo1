using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using VendorApp.Data;

namespace VendorApp.Models.Users
{
  public class UserRole : IEntity
  {
    public int ID { get; set; }
    /// <summary>
    /// The user this role belongs to
    /// </summary>
    /// <value></value>
    [Required]
    public VendorAppUser User { get; set; }

    /// <summary>
    /// The role assigned to the user
    /// </summary>
    /// <value></value>
    [Required]
    public Role Role { get; set; }
  }
}