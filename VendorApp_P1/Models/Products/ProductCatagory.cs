using System.ComponentModel.DataAnnotations;

using VendorApp.Data;

namespace VendorApp.Models.Products
{
  /// <summary>
  /// The product that will be stored inside each location
  /// </summary>
  public class ProductCatagory : IEntity
  {

    public int ID { get; set; }
    /// <summary>
    /// The product that includes this catagory
    /// </summary>
    /// <value></value>
    [Required]
    public Product Product { get; set; }
    /// <summary>
    /// The Catagory of the product
    /// </summary>
    /// <value></value>
    [Required]
    public Catagory Catagory { get; set; }
  }
}