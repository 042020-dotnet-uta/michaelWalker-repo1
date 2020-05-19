using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using VendorApp.Data;
using VendorApp.Data.EFCore;

using VendorApp.Models.Locations;
using VendorApp.Models.Orders;
using VendorApp.Models.ViewModels;

namespace VendorApp.Controllers
{
  public class LocationController : Controller
  {
    private readonly EFCoreLocationRepository locationRepo;

    public LocationController(IRepository<Location> repository)
    {
      locationRepo = repository as EFCoreLocationRepository;
    }

    /// <summary>
    /// Presents a list of all known locations to the user
    /// </summary>
    public async Task<IActionResult> Index()
    {
      List<Location> locations = await locationRepo.GetAll();

      return View(locations);
    }

    /// <summary>
    /// Presents all orders that were made from a location
    /// </summary>
    /// <param name="id">The location's id</param>
    public async Task<IActionResult> Orders(int? id)
    {
      if (id == null)
      {
        return BadRequest();
      }
      // get locatio by id
      Location location = await locationRepo.Get(id ?? -1);

      if (location == null)
      {
        return BadRequest();
      }

      // get location order items
      List<OrderItem> locationOrderItems =
        await locationRepo.getOrderItemsFromLocation(id ?? -1) as List<OrderItem>;

      return View(new LocationOrders
      {
        LocationName = location.Name,
        OrderItems = locationOrderItems
      });
    }
  }
}