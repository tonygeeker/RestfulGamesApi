using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestfulGamesApi.DataAccessLayer.Contexts;
using RestfulGamesApi.DataAccessLayer.Interfaces;
using RestfulGamesApi.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RestfulGamesApi.DataAccessLayer.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : BaseModel
    {
        internal OnlineCasinoContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(OnlineCasinoContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> All()
        {
            return dbSet.AsQueryable();
        }

        /// <summary>
        /// Returns a set of entities which match the given filter, ordered by the given 
        /// </summary>
        /// <param name="filter">Lambda expression determining the filter to be applied on the entities (ex. student => student.LastName == "Smith")</param>
        /// <param name="orderBy">Lambda expression declaring the orderBy expression (ex. q => q.OrderBy(s => s.LastName))</param>
        /// <param name="includeProperties">Comma-delimited list of navigation properties for eager loading</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        /// <summary>
        /// Returns a Queryable of entities which match the given filter, ordered by the given 
        /// </summary>
        /// <param name="filter">Lambda expression determining the filter to be applied on the entities (ex. student => student.LastName == "Smith")</param>
        /// <param name="orderBy">Lambda expression declaring the orderBy expression (ex. q => q.OrderBy(s => s.LastName))</param>
        /// <param name="includeProperties">Comma-delimited list of navigation properties for eager loading</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }

        /// <summary>
        /// Returns the total number of entities that satisfy the given filter
        /// </summary>
        /// <param name="filter">Lambda expression determining the filter to be applied on the entities (ex. student => student.LastName == "Smith")</param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            var currentDate = DateTime.Now;

            entity.DateCreated = currentDate;
            entity.DateUpdated = currentDate;

            return dbSet.Add(entity).Entity;
        }

        public void Insert(List<TEntity> entities)
        {
            var currentDate = DateTime.Now;

            foreach (var entity in entities)
            {
                entity.DateCreated = currentDate;
                entity.DateUpdated = currentDate;
            }

            dbSet.AddRange(entities);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete == null) return;
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(List<TEntity> entitiesToDelete)
        {
            //if (context.Entry(entityToDelete).State == EntityState.Detached)
            //{
            //    dbSet.Attach(entityToDelete);
            //}
            //dbSet.Remove(entityToDelete);
            dbSet.RemoveRange(entitiesToDelete);
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = context.Entry<TEntity>(entityToUpdate);

            entityToUpdate.DateUpdated = DateTime.Now;
            if (entry.State == EntityState.Detached)
            {
                var set = context.Set<TEntity>();

                TEntity attachedEntity = set.Find(entityToUpdate.Id);  // You need to have access to key

                //Set Previous DateCreated
                entityToUpdate.DateCreated = attachedEntity.DateCreated;

                if (attachedEntity != null)
                {
                    var attachedEntry = context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }

            return entityToUpdate;
        }

        public virtual void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region IDisposable Members

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
