using RestfulGamesApi.DataAccessLayer.Models;
using System.Linq.Expressions;

namespace RestfulGamesApi.BusinessServiceLayer.Interfaces
{
    public interface IService<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        /// Get a selected extiry by the object primary key ID
        /// </summary>
        /// <param name="id">Primary key ID</param>
        TEntity GetSingle(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "");

        /// <summary> 
        /// Add entity to the repository 
        /// </summary> 
        /// <param name="entity">the entity to add</param> 
        /// <returns>The added entity</returns> 
        TEntity Add(TEntity entity);

        /// <summary> 
        /// Add a list of entities to the repository 
        /// </summary> 
        /// <param name="entities">the entities to add</param> 
        void Add(List<TEntity> entities);

        /// <summary> 
        /// Mark entity to be deleted within the repository 
        /// </summary> 
        /// <param name="entity">The entity to delete</param> 
        void Delete(TEntity entity, bool permanentDelete = false);

        /// <summary>
        /// Delete range of entities
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        void Delete(List<TEntity> entitiesToDelete);

        /// <summary>
        /// Mark entity to be deleted within the repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permanentDelete"></param>
        void DeleteById(object id, bool permanentDelete = false);

        /// <summary> 
        /// Updates entity within the the repository 
        /// </summary> 
        /// <param name="entity">the entity to update</param> 
        /// <returns>The updates entity</returns> 
        TEntity Update(TEntity entity);

        /// <summary> 
        /// Load the entities using a linq expression filter
        /// </summary> 
        /// <typeparam name="E">the entity type to load</typeparam> 
        /// <param name="where">where condition</param> 
        /// <returns>the loaded entity</returns> 
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, bool includeDisabled = false, string includeProperties = "");

        /// <summary> 
        /// Load the entities using a linq expression filter as a Queryable
        /// </summary> 
        /// <typeparam name="E">the entity type to load</typeparam> 
        /// <param name="where">where condition</param> 
        /// <returns>the loaded entity</returns> 
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, bool includeDisabled = false, string includeProperties = "");

        /// <summary>
        /// Get all the element of this repository
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetAll(bool includeDisabled = false);

        /// <summary> 
        /// Query entities from the repository that match the linq expression selection criteria
        /// </summary> 
        /// <typeparam name="E">the entity type to load</typeparam> 
        /// <param name="where">where condition</param> 
        /// <returns>the loaded entity</returns> 
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereCondition);

        /// <summary>
        /// Count using a filer
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> whereCondition, bool includeDisabled = false);

        /// <summary>
        /// All item count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        int Count(bool includeDisabled = false);

        /// <summary>
        /// Save Changes
        /// </summary>
        void SaveChanges();
    }
}
