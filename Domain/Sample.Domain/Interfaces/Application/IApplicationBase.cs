using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Repositories;
using System.Collections.Generic;

namespace Sample.Domain.Interfaces.Application
{
    /// <summary>
    /// Interface of the application base, that contains basic methods of a CRUD
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public interface IApplicationBase<T>
         where T : EntityBase
    {
        /// <summary>
        /// Add a new entity to the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        T Add(T entity);
        /// <summary>
        /// Delete an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(int id);
        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        IEnumerable<T> Get();
        /// <summary>
        /// Returns an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        T GetById(int id);
        /// <summary>
        /// Searches a list of entities based on a query filter
        /// </summary>
        /// <param name="filter">Query Filter</param>
        /// <returns>List of Entities</returns>
        PagedResult<T> Search(QueryFilter entity);
        /// <summary>
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        T Update(T entity);
    }
}
