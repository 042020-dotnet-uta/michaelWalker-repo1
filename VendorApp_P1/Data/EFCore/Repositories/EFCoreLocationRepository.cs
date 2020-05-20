using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using VendorApp.Models.Locations;
using VendorApp.Models.Products;
using VendorApp.Models.Orders;
using VendorApp.Models.ApiModels;

namespace VendorApp.Data.EFCore
{
  public class EFCoreLocationRepository : EFCoreRepository<Location, P1ProtoDBContext>
  {
    private readonly P1ProtoDBContext context;

    public EFCoreLocationRepository(P1ProtoDBContext ctx) : base(ctx)
    {
      context = ctx;
    }

    // Now we can

    // TODO: Add/Update

    // TODO: Restock Inventory

    // TODO: Remove inventory
    /// <summary>
    /// Updates the locations inventory to decrease the quantity of a specified product
    /// </summary>
    /// <param name="amountToRemove">The amount to remove from the inventory</param>
    /// <param name="locationName">Name of the location</param>
    /// <param name="productName">Name of the product</param>
    /// <returns>The updated location inventory with it's know quantity</returns>
    public async Task<LocationInventory> RemoveInventory(int amountToRemove, string locationName, string productName)
    {
      LocationInventory locationInventoryToUpdate =
        await context.LocationInventoryRecords
                .FirstOrDefaultAsync(pI => pI.Product.Name == productName && pI.Location.Name == locationName);

      // Check if recor exists
      if (locationInventoryToUpdate == null)
      {
        return null;
      }

      // check if location has the amount before we remove
      // TODO: throw an exception instead
      if (locationInventoryToUpdate.Quanitity - amountToRemove < 0)
      {
        return null;
      }

      // update DB
      locationInventoryToUpdate.Quanitity -= amountToRemove;
      await context.SaveChangesAsync();

      return locationInventoryToUpdate;
    }

    /// <summary>
    /// Searches the DB for a the inventory of a location based on the location's name and the product
    /// the locatiion has in inventory
    /// </summary>
    /// <param name="locationName">Name of the location</param>
    /// <param name="productName">Name of the product</param>
    /// <returns>The location inventory model</returns>
    public async Task<LocationInventory> GetLocationInventoryByProductAndLocationName(string locationName, string productName)
    {

      return await context.LocationInventoryRecords.AsNoTracking()
                      .Include(lI => lI.Location)
                      .Include(lI => lI.Product)
                      .FirstOrDefaultAsync(lI => lI.Product.Name == productName && lI.Location.Name == locationName);
    }

    /// <summary>
    /// Retrieve a list of all known locations and a specified target location's inventory.
    /// </summary>
    /// <param name="locationId">Location's unique Id</param>
    /// <returns>An InventoryInfo ApiModel</returns>
    public async Task<InventoryInfo> GetInventroyInfo(int locationId)
    {
      List<Location> locations = await GetAll();
      // Retrieve a list of the target location's inventory 
      List<LocationInventory> targetLocationInventoryRecords = await
        context.LocationInventoryRecords
        .Include(lIR => lIR.Product)
        .Where(lIR => lIR.Location.ID == locationId).ToListAsync();

      // get a lsit of all known catagories
      List<Catagory> catagories = await context.Catagories.ToListAsync();

      return new InventoryInfo
      {
        Locations = locations,
        TargetInventory = targetLocationInventoryRecords,
        Catagories = catagories
      };
    }

    // TODO: add docs
    public async Task<ICollection<OrderItem>> getOrderItemsFromLocation(int id)
    {
      // get location by id
      Location location = await context.Locations.AsNoTracking().FirstOrDefaultAsync(l => l.ID == id);
      if (location == null)
      {
        return null;
      }
      // return list of orderitems from orderRepo
      return await new EFCoreOrderRepository(context).GetOrderItemsByLocationName(location.Name);
    }

    // TODO: add docs
    public async Task<List<LocationInventory>> GetAllLocationInventoryRecords()
    {
      return await context.LocationInventoryRecords.ToListAsync();
    }

    // TODO: add docs
    public async Task<LocationInventory> AddLocationInventory(LocationInventory locationInventory)
    {
      context.LocationInventoryRecords.Add(locationInventory);
      await context.SaveChangesAsync();

      return locationInventory;
    }

    public async Task<LocationInventory> RemoveLocationInventoryRecord(int id)
    {
      LocationInventory locationInventory = await context.LocationInventoryRecords.FindAsync(id);

      if (locationInventory == null)
      {
        // TODO: Log that no Entity was found to delete here or in Repo
        return locationInventory;
      }

      context.LocationInventoryRecords.Remove(locationInventory);

      await context.SaveChangesAsync();

      return locationInventory;
    }
  }
}