using System.ComponentModel.DataAnnotations;

using VendorApp.Data;

namespace VendorApp.Models.Products
{
  /// <summary>
  /// The Catagory that is used to seperate a product into a 
  /// specific genre
  /// </summary>
  public class Catagory : IEntity
  {
    /// <summary>
    /// Catagory's unique ID
    /// </summary>
    /// <value></value>
    [Required]
    public int ID { get; set; }
    /// <summary>
    /// Name of the catagory (eg. Furniture)
    /// </summary>
    /// <value></value>
    [Required]
    public string CatagoryName { get; set; }
    /// <summary>
    /// The predefined color of the catagory that will be used to 
    /// help distinguish a product's catagory.
    /// </summary>
    /// <value>A 6 character string</value>
    [Required]
    [MaxLength(7)]
    public string HexColorTheme { get; set; }
  }
}