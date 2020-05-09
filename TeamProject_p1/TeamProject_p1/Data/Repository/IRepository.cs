using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamProject_p1.Data
{
  // Used implementation of repo as described in this article: 
  // https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7
  public interface IRepository<T> where T : IEntity
  {
    Task<List<T>> GetAll();
    Task<T> Get(int id);
    Task<T> Add(T entity);
    Task<T> Update(T ientity);
    Task<T> Delete(int id);
  }
}