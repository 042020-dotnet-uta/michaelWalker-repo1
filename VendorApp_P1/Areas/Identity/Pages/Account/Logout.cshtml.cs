using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using VendorApp.Models.Users;


namespace VendorApp.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LogoutModel : PageModel
  {
    private readonly SignInManager<VendorAppUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<VendorAppUser> signInManager, ILogger<LogoutModel> logger)
    {
      _signInManager = signInManager;
      _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
      await _signInManager.SignOutAsync();
      _logger.LogInformation("User logged out.");
      if (returnUrl != null)
      {
        return LocalRedirect(returnUrl);
      }
      else
      {
        return RedirectToPage();
      }
    }
  }
}
