using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using VendorApp.Data;
using VendorApp.Data.EFCore;

using VendorApp.Models.Carts;
using VendorApp.Models.Users;
using VendorApp.Models.Locations;


namespace VendorApp.Controllers
{
  [Authorize]
  public class CartController : Controller
  {
    private readonly ILogger<CartController> cartLogger;
    private readonly EFCoreCartRepository cartRepo;
    private readonly UserManager<VendorAppUser> userManager;

    public CartController(
      ILogger<CartController> logger,
      IRepository<Cart> repository,
      UserManager<VendorAppUser> userManager
    )
    {
      cartLogger = logger;
      cartRepo = repository as EFCoreCartRepository;
      this.userManager = userManager;
    }

    /// <summary>
    /// Show the cart of the currently logged in user
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      VendorAppUser vendorAppUser = await userManager.GetUserAsync(User);

      Cart userCart = await cartRepo.FindCartByUserId(userId);
      // string loggedInUserId = userManager.GetUserId;
      // return Content($"Here's a cart for ya -- {userCart.ToString()}");
      return View(userCart);
    }

    // TODO: add docs
    [HttpPost]
    public async Task<IActionResult> DeleteCartItem(int? id)
    {
      // check if id is valid
      if (id == null)
      {
        TempData["ErrorMessage"] = "Please enter valid input";
        return Redirect(Request.Headers["Referer"]);
      }
      // Remove Cart Item
      await cartRepo.RemoveItemFromCart(id ?? -1);
      // Store a flash message
      TempData["FlashMessage"] = "Item removed from cart!";
      // Redirect back to cart
      return RedirectToAction("Index", "Cart");
    }

    // TODO: add docs
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RegisterPurchase(int? id)
    {
      // check if the id is valid
      if (id == null)
      {
        TempData["ErrorMessage"] = "Please enter valid input";
        return Redirect(Request.Headers["Referer"]);
      }

      Cart registeredCart = await cartRepo.RegisterPurchase(id ?? -1);
      TempData["FlashModalMessage"] = "Order succesfully made";

      return RedirectToAction("Index");
    }

    /// <summary>
    /// Stores a cart item to the user upon request
    /// </summary>
    /// <param name="quantity">The amount of the product that the user requested</param>
    /// <param name="productName">The name of the product</param>
    /// <param name="locationName">The name of the location that obtains said product</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCartItem(int? quantity, string productName, string locationName)
    {
      // Get the user who's azdding the item to the cart
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      VendorAppUser user = await userManager.GetUserAsync(User);

      LocationInventory targetLocationInventory = await cartRepo.GetLocationInventoryByProductAndLocationName(locationName, productName);

      // Redirect user back to product page if any of the paramerters aren't valid
      if (quantity == null || productName == null || locationName == null || targetLocationInventory == null)
      {
        TempData["ErrorMessage"] = "Please enter valid input";
        return Redirect(Request.Headers["Referer"]);
      }

      if (quantity > targetLocationInventory.Quanitity || quantity <= 0)
      {
        TempData["ErrorMessage"] = "Please enter a valid quantity";
        return Redirect(Request.Headers["Referer"]);
      }

      // Add item to cart
      await cartRepo.AddItemToCart(userId, productName, locationName, quantity ?? -1);

      // Document changes
      TempData["FlashMessage"] = "Added to cart";
      cartLogger.LogInformation($"{productName} was added to {user.UserName}'s cart");

      // Take client back to list of products

      return RedirectToAction("Index", "Product");
    }

    // TODO: create a function to verify the cart being accessed is the user's cart
  }

}