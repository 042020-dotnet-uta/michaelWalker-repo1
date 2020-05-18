using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using VendorApp.Models.Products;
using VendorApp.Models.Locations;
using VendorApp.Models.ViewModels;
using VendorApp.Models.ApiModels;
using VendorApp.Data;
using VendorApp.Data.EFCore;

namespace VendorApp.Controllers
{
  public class ProductController : Controller
  {
    private readonly ILogger<ProductController> logger;
    private readonly EFCoreProductRepository productRepo;

    public ProductController(IRepository<Product> repository, ILogger<ProductController> logger = null)
    {
      this.productRepo = repository as EFCoreProductRepository;
      this.logger = logger;
    }
    // public string Index()
    // {
    //   return "Help";
    // }

    /// <summary>
    /// Displays a view of all the known products
    /// </summary>
    public IActionResult Index()
    {
      return View();
    }

    /// <summary>
    /// Displays the detail of a specific product
    /// </summary>
    /// <param name="id">The Product's id</param>
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null) return NotFound();

      Product product = await productRepo.Get(id ?? -1);

      if (product == null) return NotFound();

      return View(product);
    }

    /// <summary>
    /// Returns a view to the client that will display a list of Product's
    /// for a given Location
    /// </summary>
    /// <param name="id">The Id used to retrieve the location</param>
    public async Task<IActionResult> Show(int? id)
    {
      // validate given id and stop process if null and return NotFound
      if (id == null)
      {
        return NotFound();
      }

      List<LocationInventory> locationInventoryRecords =
        await productRepo.GetLocationInventoryByLocationId(id ?? -1) as List<LocationInventory>;

      // Return the Location with it's LocatioInventory

      return View(locationInventoryRecords);
    }

    /// <summary>
    /// Provides the view with data regarding a location's inventory of a specific product
    /// as well as a list of other location's inventory that have the product.
    /// </summary>
    /// <param name="id">Id of the product</param>
    /// <param name="locationId">Location's specific id</param>
    /// <returns>
    /// A view with data on a location's product and several other location's invenories
    /// with this product.
    /// </returns>
    public async Task<IActionResult> ShowProduct(int? id, int? locationId)
    {
      if(id == null || locationId == null)
      {
        return NotFound();
      }

      ProductInventoryInfo pII = await productRepo.GetProductInventoryInfo(id ?? -1, locationId ?? -1);

      if(pII == null){
        return NotFound();
      }

      return View(pII);
    }

    // TODO: add docs
    [HttpGet]
    public async Task<ActionResult<InventoryInfo>> GetProductInfo(int? locationId)
    {
      // Product mockProduct = new Product{
      //   ID = someId ?? -1,
      //   Name = "Cool product"
      // };

      if(locationId == null)
      {
        return BadRequest("Uhm");
      }

      return CreatedAtAction(nameof(GetProductInfo), await productRepo.GetInventroyInfo(locationId ?? -1));
    }

    /// <summary>
    /// Responds with a list of all known locations and the inventory records for a specified location
    /// </summary>
    /// <param name="locationId">Location's unique id used to retrieve it's inventory records</param>
    /// <returns></returns>
    // [HttpGet]
    // public async ActionResult<InventoryInfo> GetInventoryInfo(int? locationId)
    // {

    // }
  }
}