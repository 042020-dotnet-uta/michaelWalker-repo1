using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VendorApp.Data;
using VendorApp.Models.Users;
using VendorApp.Models.Locations;
using VendorApp.Models.Products;
using System;
using System.Linq;

namespace VendorApp.Models
{
  public static class SeedData
  {
    public static void InitDB(IServiceProvider serviceProvider)
    {
      using var ctx = new P1ProtoDBContext(
        serviceProvider
        .GetRequiredService<DbContextOptions<P1ProtoDBContext>>()
        );


      if (!ctx.Users.Any() || !ctx.Locations.Any() || !ctx.Products.Any())
      {
        // * Make users
        if (!ctx.Users.Any())
        {
          ctx.Users.AddRange(
            new VendorAppUser
            {
              UserName = "Admin",
              Email = "admin@vendorapp.com",
            },
            new VendorAppUser
            {
              UserName = "User1",
              Email = "user1@vendorapp.com",
            },
            new VendorAppUser
            {
              UserName = "User2",
              Email = "user2@vendorapp.com",
            }
          );

          ctx.SaveChanges();
        }

        // * Make Catagories

        if (!ctx.Catagories.Any())
        {
          ctx.AddRange(
            new Catagory {
              CatagoryName = "Electronics",
              HexColorTheme = "#42f5c5"
            },
            new Catagory {
              CatagoryName = "Clothing",
              HexColorTheme = "#427bf5"
            },
            new Catagory {
              CatagoryName = "Entertainment",
              HexColorTheme = "#f59b42"
            }
          );
        }

        // * Make Products

        if (!ctx.Products.Any())
        {
          ctx.Products.AddRange(
              new Product
              {
                Name = "Book",
                FAClass = "fas fa-book",
                Catagory = "Entertainment"
              },
              new Product
              {
                Name = "Chess",
                FAClass = "fas fa-chess",
                Catagory = "Entertainment"
              },
              new Product
              {
                Name = "Battery",
                FAClass = "fas fa-battery-full",
                Catagory = "Electronics"
              },
              new Product
              {
                Name = "Phone",
                FAClass = "fas fa-mobile-alt",
                Catagory = "Electronics"
              },
              new Product
              {
                Name = "Hat",
                FAClass = "fas fa-hat-cowboy",
                Catagory = "Clothing"
              },
              new Product
              {
                Name = "Tie",
                FAClass = "fab fa-black-tie",
                Catagory = "Clothing"
              },
              new Product
              {
                Name = "Drink",
                FAClass = "fas fa-cocktail",
                Catagory = "Entertainment"
              },
              new Product
              {
                Name = "Discord",
                FAClass = "fab fa-discord",
                Catagory = "Entertainment"
              },
              new Product
              {
                Name = "Fighter Jet",
                FAClass = "fas fa-fighter-jet",
                Catagory = "Electronics"
              },
              new Product
              {
                Name = "Bike",
                FAClass = "fas fa-bicycle",
                Catagory = "Entertainment"
              }
          );

          ctx.SaveChanges();
        }

        // * Make Locations

        if (!ctx.Locations.Any())
        {
          // Fetch some products

          List<Product> products = ctx.Products.ToList();

          // Setup for locations
          string[] stores = { "Store1", "Store2", "Store3" };
          List<Location> newLocations = new List<Location>();

          foreach (string store in stores)
          {
            // make a new location

            Location newLocation = new Location
            {
              Name = store
            };

            List<LocationInventory> locationInventoryRecords = new List<LocationInventory>();

            newLocations.Add(newLocation);
          }

          // Add list to DBContext
          ctx.Locations.AddRange(newLocations);

          ctx.SaveChanges();



        }
      }

      if (!ctx.LocationInventoryRecords.Any())
      {
        // * Add inventory records
        Random rnd = new Random();
        // get locations
        List<Location> locations = ctx.Locations.ToList();
        // get products
        List<Product> productsToAdd = ctx.Products.ToList();

        List<LocationInventory> locationInventoryRecords = new List<LocationInventory>();
        // TODO: add inventory after
        // foreach (var product in products)
        // {
        //   // Setup Location's inventory
        //   Random rnd = new Random();
        //   newLocation.ProductInventoryRecords.Add(
        //     new LocationInventory
        //     {
        //       Location = newLocation,
        //       Product = product,
        //       Quanitity = rnd.Next(10)
        //     }
        //   );
        // }

        foreach (var location in locations)
        {
          foreach (var product in productsToAdd)
          {
            locationInventoryRecords.Add(new LocationInventory
            {
              Location = location,
              Product = product,
              Quanitity = rnd.Next(50)
            });
          }
        }

        ctx.LocationInventoryRecords.AddRange(locationInventoryRecords);
        ctx.SaveChanges();
      }
    }
  }
}