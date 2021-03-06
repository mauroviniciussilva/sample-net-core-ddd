﻿using Sample.Domain.Entities;
using System.Linq;

namespace Sample.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface of the repository base, that contains basic methods of a CRUD and manipulation of the registries from the database
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public interface IRepositoryBase<TEntity>
       where TEntity : EntityBase
    {
        /// <summary>
        /// Add a new entity to the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        TEntity Add(TEntity entity);
        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        IQueryable<TEntity> GetAllActive();

        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Returns an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        TEntity GetById(int id);
        /// <summary>
        /// Commit the changes to the database
        /// </summary>
        /// <remarks><![CDATA[
        /// You need to use that method to the changes have some effect in the database
        /// ]]>
        /// </remarks>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        TEntity Update(TEntity entity);
    }
}