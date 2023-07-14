using System.Linq.Expressions;

namespace RestfulGamesApi.DataAccessLayer.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        int Count(Expression<Func<TEntity, bool>> filter = null);

        TEntity GetByID(object id);

        TEntity Insert(TEntity entity);

        void Insert(List<TEntity> entities);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Delete(List<TEntity> entitiesToDelete);

        TEntity Update(TEntity entityToUpdate);

        void SaveChanges();

        /// <summary>
        /// Gets all objects from database
        /// </summary>
        IQueryable<TEntity> All();
    }
}
