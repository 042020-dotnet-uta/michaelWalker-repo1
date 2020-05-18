using System.Collections.Generic;
using System.Threading.Tasks;

namespace VendorApp.Data
{
  /// <summary>
  /// Collection of methods for retrieving and handling data from the DB
  /// </summary>
  /// <typeparam name="T">TODO: add desc</typeparam>
  public interface IRepository<T> where T : IEntity
  {
    /// <summary>
    /// Store all entities into a list
    /// </summary>
    /// <returns>A list of all entities of a given type</returns>
    Task<List<T>> GetAll();
    /// <summary>
    /// Return an entity using a unique id
    /// </summary>
    /// <param name="id">Unique id of the specified entitiy</param>
    /// <returns>The found entity or null if none was found</returns>
    Task<T> Get(int id);
    /// <summary>
    /// Inserts the entity's properties into the DB
    /// </summary>
    /// <param name="entity">The entity to insert</param>
    /// <returns>The inserted entity</returns>
    Task<T> Add(T entity);
    /// <summary>
    /// Updates an existing entity found within the DB
    /// </summary>
    /// <param name="entity">An entity with updated properties</param>
    /// <returns>The updated entity</returns>
    Task<T> Update(T entity);
    /// <summary>
    /// Finds an entity by id then deletes it
    /// </summary>
    /// <param name="id">ID of the entity to find</param>
    /// <returns>The deleted entity</returns>
    Task<T> Delete(int id);
  }
}