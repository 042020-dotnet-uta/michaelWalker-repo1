using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using VendorApp.Data;
using VendorApp.Data.EFCore;

using VendorApp.Models.Orders;
using VendorApp.Models.Users;
using VendorApp.Models.ViewModels;


namespace VendorApp.Controllers
{
  [Authorize]
  public class OrderController : Controller
  { 
    private readonly ILogger<OrderController> orderLogger;

    private readonly EFCoreOrderRepository orderRepo;

    private readonly UserManager<VendorAppUser> userManager;


    public OrderController(IRepository<Order> repository, UserManager<VendorAppUser> userManager){
      orderRepo = repository as EFCoreOrderRepository;
      this.userManager = userManager;
    }

    /// <summary>
    /// Dispalys a list of orders for the current user
    /// </summary>
    /// <returns>A view to present the user's order</returns>
    public async Task<IActionResult> Customer(){
      // Use logged in user to get orders by userId
      VendorAppUser user = await userManager.GetUserAsync(User);

      List<Order> userOrders = await orderRepo.GetOrdersByUserId(user.Id) as List<Order>;
      // return list to view
      return View(new UserOrders{
        UserName = user.UserName,
        Orders = userOrders
      });
    }


    // public async Task<IActionResult> Location(string locationName)
    // {
    //   // direct user to bad request if invalid locationName was given
    //   if(locationName == null)
    //   {
    //     return BadRequest();
    //   }
    // }
  }
}