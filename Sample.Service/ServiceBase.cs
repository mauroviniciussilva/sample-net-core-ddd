using Sample.Domain.Entities;
using Sample.Domain.Exceptions;
using Sample.Domain.Interfaces;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Sample.Service
{
    /// <summary>
    /// A generic class of service that contains basic methods of a CRUD
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public class ServiceBase<TEntity> : IServiceBase<TEntity>
        where TEntity : EntityBase
    {
        #region [ Properties ]

        internal IRepositoryBase<TEntity> _repository;
        internal IUserHelper _userHelper;

        #endregion

        #region [ Constructor ]

        public ServiceBase(IRepositoryBase<TEntity> repository, IUserHelper userHelper)
        {
            _repository = repository;
            _userHelper = userHelper;
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
            entity.UserCreationId = _userHelper.LoggedUser.Id;
            entity.CreationDate = DateTime.Now;
            entity.Active = true;

            if (!entity.IsValid())
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(Add), "Invalid entity");

            entity = _repository.Add(entity);

            _repository.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Delete an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        public void DeleteById(int id)
        {
            if (id == 0)
            {
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(DeleteById), "The ID can't be zero", nameof(TEntity));
            }

            var entity = _repository.GetById(id);

            _repository.Inactivate(id);

            _repository.SaveChanges();
        }

        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        public virtual IEnumerable<TEntity> Get() => _repository.GetAll();

        /// <summary>
        /// Returns an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Searches a list of entities based on a query filter
        /// </summary>
        /// <param name="filter">Query Filter</param>
        /// <returns>List of Entities</returns>
        public virtual PagedResult<TEntity> Search(QueryFilter filter)
        {
            return _repository.Search(filter);
        }

        /// <summary>
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public virtual TEntity Update(TEntity entity)
        {
            entity.UserModificationId = _userHelper.LoggedUser.Id;
            entity.ModificationDate = DateTime.Now;

            if (!entity.IsValid())
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(Add), "Invalid entity");

            _repository.Update(entity);

            _repository.SaveChanges();

            return entity;
        }

        #endregion
    }
}