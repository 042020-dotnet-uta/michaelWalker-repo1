using System.ComponentModel.DataAnnotations;

using VendorApp.Data;

namespace VendorApp.Models.Products
{
  /// <summary>
  /// The product that will be stored inside each location
  /// </summary>
  public class Product : IEntity
  {
    /// <summary>
    /// Unique Identifier
    /// </summary>
    /// <value></value>
    [Key]
    public int ID { get; set; }
    /// <summary>
    /// The products Name
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(50)]

    public string Name { get; set; }
    /// <summary>
    /// The products Catagory
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(50)]
    public string Catagory { get; set; } // TODO: change toe CatagoryName
    
    /// <summary>
    /// A Font-Awesome classname that is used to display an icon.
    /// This is basically a subsitute for image urls from an asset cdn.
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(50)]
    public string FAClass { get; set; }

  }
}