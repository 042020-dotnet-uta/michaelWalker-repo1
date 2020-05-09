using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamProject_p1.Data;

namespace TeamProject_p1.Controllers
{
  public abstract class DBController<TEntity, TRepository> : Controller
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
  {
    private readonly TRepository repository;

    public DBController(TRepository repository)
    {
      this.repository = repository;
    }

    
  }
}