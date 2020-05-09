using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamProject_p1.Data.EFCore
{
  // Implementaion of EFCoreRepository inspired by tutorial:
  // https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7
  public abstract class EFCoreRepository<TEntity, TContext> : IRepository<TEntity>
  where TEntity : class, IEntity
  where TContext : ProjectDbContext
  {
    private readonly TContext context;
    public EFCoreRepository(TContext context)
    {
      this.context = context;
    }
    public async Task<TEntity> Add(TEntity entity)
    {
      context.Set<TEntity>().Add(entity);
      await context.SaveChangesAsync();
      return entity;
    }

    public async Task<TEntity> Delete(int id)
    {
      var entity = await context.Set<TEntity>().FindAsync(id);
      if (entity == null)
      {
        return entity;
      }

      context.Set<TEntity>().Remove(entity);
      await context.SaveChangesAsync();

      return entity;
    }

    public async Task<TEntity> Get(int id)
    {
      return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> GetAll()
    {
      return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
      context.Entry(entity).State = EntityState.Modified;
      await context.SaveChangesAsync();
      return entity;
    }
  }
}