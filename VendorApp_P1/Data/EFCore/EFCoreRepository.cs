using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VendorApp.Data.EFCore
{
  // Implementaion of EFCoreRepository inspired by tutorial:
  // https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7
  public abstract class EFCoreRepository<TEntity, TContext> : IRepository<TEntity>
  where TEntity : class, IEntity
  where TContext : P1ProtoDBContext
  {
    // instance of DBContext
    private readonly TContext ctx;

    public EFCoreRepository(TContext ctx)
    {
      this.ctx = ctx;
    }

    /// <summary>
    /// Store all entities into a list
    /// </summary>
    /// <returns>A list of all entities of a given type</returns>
    public async Task<List<TEntity>> GetAll()
    {
      return await ctx.Set<TEntity>().ToListAsync();
    }

    /// <summary>
    /// Return an entity using a unique id, if entity is not found, reutrns null
    /// </summary>
    /// <param name="id">Unique id of the specified entitiy</param>
    /// <returns>The found entity or null if none was found</returns>
    public async Task<TEntity> Get(int id)
    {
      return await ctx.Set<TEntity>().FindAsync(id);
    }

    /// <summary>
    /// Inserts the entity's properties into the DB
    /// </summary>
    /// <param name="entity">The entity to insert</param>
    /// <returns>The inserted entity</returns>
    public async Task<TEntity> Add(TEntity entity)
    {
      try
      {
        ctx.Set<TEntity>().Add(entity);
      }
      catch (DbUpdateException e)
      {
        Console.WriteLine("An issue occured when trying make an insertion to the DB");
        Console.WriteLine(e.InnerException.Message);
      }
      await ctx.SaveChangesAsync();
      return entity;
    }

    public async Task<TEntity> Update(TEntity entity)
    {
      ctx.Set<TEntity>().Update(entity);
      await ctx.SaveChangesAsync();
      return entity;
    }

    public virtual async Task<TEntity> Delete(int id)
    {
      TEntity entity = await ctx.Set<TEntity>().FindAsync(id);

      if (entity == null)
      {
        // TODO: Log that no Entity was found to delete here or in Repo
        return entity;
      }

      ctx.Set<TEntity>().Remove(entity);

      await ctx.SaveChangesAsync();

      return entity;
    }
  }
}