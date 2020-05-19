using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using VendorApp.Data;
using VendorApp.Data.EFCore;
using VendorApp.Models.Users;
using VendorApp.Models.Products;
using VendorApp.Models.Locations;
using VendorApp.Models.Carts;
using VendorApp.Models.Orders;


namespace VendorApp
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();

      services.AddControllersWithViews();
      services.AddControllersWithViews().AddNewtonsoftJson(opts =>
            opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
      services.AddRazorPages();

      // Initialize DBCOntext for EFCore
      services.AddDbContext<P1ProtoDBContext>(otps =>
          otps.UseSqlServer(Configuration.GetConnectionString("ProjectDbContext")));

      // Register repositories with DI system
      services.AddScoped<IRepository<VendorAppUser>, EFCoreUserRepository>();
      services.AddScoped<IRepository<Product>, EFCoreProductRepository>();
      services.AddScoped<IRepository<Location>, EFCoreLocationRepository>();
      services.AddScoped<IRepository<Cart>, EFCoreCartRepository>();
      services.AddScoped<IRepository<Order>, EFCoreOrderRepository>();

      // * Identity
      services.AddDefaultIdentity<VendorAppUser>(opts =>
        opts.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<P1ProtoDBContext>();

      services.Configure<IdentityOptions>(options =>
    {
      // Password settings.
      options.Password.RequireDigit = false;
      options.Password.RequireLowercase = false;
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequireUppercase = false;
      // options.Password.RequiredLength = 6;
      // options.Password.RequiredUniqueChars = 1;

      // Lockout settings.
      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
      options.Lockout.MaxFailedAccessAttempts = 5;
      options.Lockout.AllowedForNewUsers = true;

      // User settings.
      options.User.AllowedUserNameCharacters =
      "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
      options.User.RequireUniqueEmail = false;
    });

      services.ConfigureApplicationCookie(options =>
      {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // app.UseCors(opts => opts.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());    // TODO: remove later

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
