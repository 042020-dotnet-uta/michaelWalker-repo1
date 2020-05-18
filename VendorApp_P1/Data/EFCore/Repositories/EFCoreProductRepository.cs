using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using VendorApp.Models.Products;
using VendorApp.Models.Locations;
using VendorApp.Models.ViewModels;
using VendorApp.Models.ApiModels;

namespace VendorApp.Data.EFCore
{
  public class EFCoreProductRepository : EFCoreRepository<Product, P1ProtoDBContext>
  {
    private readonly P1ProtoDBContext context;

    public EFCoreProductRepository(P1ProtoDBContext ctx) : base(ctx)
    {
      context = ctx;
    }

    // Now we can

    // TODO: Assign catagory to product

    /// <summary>
    /// Find the LocationInventoryRecords for a product 
    /// based on the Location's given Id
    /// </summary>
    /// <param name="id">The given Location's Id</param>
    /// <returns>A list of Inventory Records</returns>
    public async Task<ICollection<LocationInventory>> GetLocationInventoryByLocationId(int id)
    {
      return await context.LocationInventoryRecords
        .AsNoTracking()
        .Where(lI => lI.Location.ID == id)
        .Include(pI => pI.Location)
        .Include(pI => pI.Product)
        .ToListAsync();
    }

    /// <summary>
    /// Fetches data from DB that will pull the inventories for several location
    /// relative to a specified product
    /// </summary>
    /// <param name="productId">The product's id used to retrive the inventory records</param>
    /// <param name="productId">The location's id TODO: continue</param>
    /// <returns>
    /// ProductInventoryInfo - A model with a targeted inventory record and a list
    /// of all locations that have the product in it's inventory
    /// </returns>
    public async Task<ProductInventoryInfo> GetProductInventoryInfo(int productId, int? locationId)
    {
      // Get all location inventory records that include this product
      List<LocationInventory> locationInventoryRecords =
        await context.LocationInventoryRecords
          .AsNoTracking()
          .Include(lI => lI.Product)
          .Include(lI => lI.Location)
          .Where(lI => lI.Product.ID == productId)
          .ToListAsync();

      // createa a new instance of PoductInventoryInfo model
      // specify the targeted inventory and store all retreived 
      // inventory records to a list
      return new ProductInventoryInfo
      {
        TargetLocationInventory = locationInventoryRecords
                                    .FirstOrDefault(lI => lI.Location.ID == locationId && lI.Product.ID == productId),
        ProductRelatedLocationInventoryRecords = locationInventoryRecords
      };
    }

    /// <summary>
    /// Retrieve a list of all known locations and a specified target location's inventory.
    /// </summary>
    /// <param name="locationId">Location's unique Id</param>
    /// <returns>An InventoryInfo ApiModel</returns>
    public async Task<InventoryInfo> GetInventroyInfo(int locationId)
    {
      return await new EFCoreLocationRepository(context).GetInventroyInfo(locationId);
    }
  }
}