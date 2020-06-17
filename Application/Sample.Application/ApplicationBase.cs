using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Application;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace Sample.Application
{
    /// <summary>
    /// A generic class of application that contains basic methods of a CRUD
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public class ApplicationBase<TEntity> : IApplicationBase<TEntity>
         where TEntity : EntityBase
    {
        #region [ Properties ]

        internal IServiceBase<TEntity> _service;

        #endregion

        #region [ Constructor ]

        public ApplicationBase(IServiceBase<TEntity> service)
        {
            _service = service;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Add a new entity to the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public virtual TEntity Add(TEntity entity)
        {
            return _service.Add(entity);
        }

        /// <summary>
        /// Delete an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        public virtual void Delete(int id)
        {
            _service.DeleteById(id);
        }

        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        public virtual IEnumerable<TEntity> Get() => _service.Get();

        /// <summary>
        /// Returns an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(int id)
        {
            return _service.GetById(id);
        }

        /// <summary>
        /// Searches a list of entities based on a query filter
        /// </summary>
        /// <param name="filter">Query Filter</param>
        /// <returns>List of Entities</returns>
        public virtual PagedResult<TEntity> Search(QueryFilter filter)
        {
            return _service.Search(filter);
        }

        /// <summary>
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public virtual TEntity Update(TEntity entity)
        {
            return _service.Update(entity);
        }

        #endregion
    }
}