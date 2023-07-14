using RestfulGamesApi.BusinessServiceLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;
using System.Linq.Expressions;

namespace RestfulGamesApi.BusinessServiceLayer.Implementations
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : BaseModel
    {
        #region Repository Setup        

        private IRepository<TEntity> repository;
        public BaseService(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        #endregion

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return repository.Get(filter, includeProperties).Where(x => x.IsEnabled).SingleOrDefault();
        }

        public TEntity GetSingle(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return repository.Get(filter, string.Empty).Where(x => x.IsEnabled).SingleOrDefault();
        }

        public TEntity Add(TEntity entity)
        {
            return repository.Insert(entity);
        }

        public void Add(List<TEntity> entities)
        {
            repository.Insert(entities);
        }

        public void Delete(TEntity entity, bool permanentDelete = false)
        {
            if (permanentDelete)
            {
                repository.Delete(entity);
            }
            else
            {
                //Soft Delete
                entity.IsEnabled = false;
                Update(entity);
            }
        }

        public void Delete(List<TEntity> entitiesToDelete)
        {
            repository.Delete(entitiesToDelete);
        }

        public void DeleteById(object id, bool permanentDelete = false)
        {
            var entity = GetById(id);
            Delete(entity, permanentDelete);
        }

        public virtual TEntity Update(TEntity entity)
        {
            return repository.Update(entity);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, bool includeDisabled = false, string includeProperties = "")
        {
            var query = repository.Get(filter, includeProperties);

            if (!includeDisabled)
            {
                query = query.Where(x => x.IsEnabled);
            }

            return query;
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, bool includeDisabled = false, string includeProperties = "")
        {
            var query = repository.GetQueryable(filter, includeProperties);

            if (!includeDisabled)
            {
                query = query.Where(x => x.IsEnabled);
            }

            return query;
        }

        public TEntity GetById(object id)
        {
            return repository.GetByID(id);
        }

        public IList<TEntity> GetAll(bool includeDisabled = false)
        {
            var query = repository.All();

            if (!includeDisabled)
            {
                query = query.Where(x => x.IsEnabled);
            }

            return query.ToList();
        }

        public IQueryable<TEntity> Query(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return repository.Get(filter, string.Empty).AsQueryable().Where(x => x.IsEnabled); ;
        }

        public int Count(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, bool includeDisabled = false)
        {
            var query = this.Query(filter);

            if (!includeDisabled)
            {
                query = query.Where(x => x.IsEnabled);
            }

            return query.Count();
        }

        public int Count(bool includeDisabled = false)
        {
            var query = repository.GetQueryable();

            if (!includeDisabled)
            {
                query = query.Where(x => x.IsEnabled);
            }

            return query.Count();
        }

        public void SaveChanges()
        {
            repository.SaveChanges();
        }

    }
}
