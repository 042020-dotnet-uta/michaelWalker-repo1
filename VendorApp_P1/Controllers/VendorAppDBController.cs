
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


using VendorApp.Data;
using VendorApp.Models.Users;

namespace VendorApp.Controllers
{
  public abstract class VendorAppDBController<TEntity, TRepository> : Controller
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
  {
    private readonly TRepository repository;
    private readonly ILogger<Controller> logger;
    private readonly UserManager<VendorAppUser> userManager;

    public VendorAppDBController(TRepository repository)
    {
      this.repository = repository;
    }

    public VendorAppDBController(TRepository repository, ILogger<Controller> logger)
    {
      this.repository = repository;
      this.logger = logger;
    }
    // public VendorAppDBController(TRepository repository, ILogger<Controller> logger)
    // {
    //   this.repository = repository;
    //   this.logger = logger;
    // }

    // public virtual async Task<IActionResult> Index()
    // {
    //   return View(await repository.GetAll());
    // }
  }
}