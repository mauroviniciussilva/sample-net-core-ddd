using Sample.Domain.Entities;
using Sample.Domain.Exceptions;
using Sample.Domain.Interfaces;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            if (entity.UserCreationId <= 0)
                entity.UpdateUserCreationId(_userHelper.LoggedUser.Id);

            if (!entity.IsValid())
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(Add), "Invalid entity");

            entity = _repository.Add(entity);

            _repository.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Delete an entity based on its id
        /// </summary>
        /// <remarks><![CDATA[
        /// This method only inactivates the record, but database queries return only active records. This was designed 
        /// in this way to recover a record deleted by accident, for data analysis, among other possibilities
        /// ]]>
        /// </remarks>
        /// <param name="id">Entity's Id</param>
        public void DeleteById(int id)
        {
            if (id == 0)
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(DeleteById), "The ID can't be zero", nameof(TEntity));

            var entity = _repository.GetById(id);

            if (entity == null)
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(DeleteById), "Entity not found", nameof(TEntity));

            entity.Inactivate(_userHelper.LoggedUser.Id);
            _repository.Update(entity);

            _repository.SaveChanges();
        }

        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        public virtual IEnumerable<TEntity> GetAll() => _repository.GetAllActive();

        /// <summary>
        /// Returns an entity based on its id
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Searches a list of entities based on a query filter
        /// </summary>
        /// <remarks><![CDATA[
        /// As EntityFramework does not allow reflection to be used, filters on properties must be done manually. Therefore, ServiceBase 
        /// implements the basic filters in the EntityBase properties. This method can receive an IQueryable as a parameter so that it is
        /// possible to manually build the filters of each entity in the corresponding service - overriding this method, and then calling 
        /// it passing the query that already has the specific filters to the entity.
        /// ]]>
        /// </remarks>
        /// <param name="filter">Query Filter</param>
        /// <param name="query">Filtered query (see the method remarks for more info)</param>
        /// <returns>List of Entities</returns>
        public virtual PagedResult<TEntity> Search(QueryFilter filter, IQueryable<TEntity> query = null)
        {
            if (filter.Page == 0) filter.Page = 1;
            if (filter.Limit == 0) filter.Limit = 20;

            if (query == null) query = _repository.GetAll();

            foreach(var f in filter.Filters)
            {
                switch (f.Key)
                {
                    // Switch doesn't do anything but an ordinal comparison. Because of that, I've used the
                    // following solution: declare the local variable 'key', who receives the 'f.Key' value. 
                    // Further to that, the variable 'key' exists only so that the when clause to the case 
                    // statement can exist
                    case string key when nameof(EntityBase.Id).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        int.TryParse(f.Value, out var id);
                        query = query.Where(x => x.Id.Equals(id));
                        break;
                    case string key when nameof(EntityBase.UserCreationId).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        int.TryParse(f.Value, out var userCreationId);
                        query = query.Where(x => x.UserCreationId.Equals(userCreationId));
                        break;
                    case string key when nameof(EntityBase.CreationDate).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        DateTime.TryParse(f.Value, out var creationDate);
                        query = query.Where(x => x.CreationDate.Date.Equals(creationDate.Date));
                        break;
                    case string key when nameof(EntityBase.UserModificationId).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        int.TryParse(f.Value, out var userModificationId);
                        query = query.Where(x => x.UserModificationId.Equals(userModificationId));
                        break;
                    case string key when nameof(EntityBase.ModificationDate).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        DateTime.TryParse(f.Value, out var modificationDate);
                        query = query.Where(x => x.ModificationDate.Value.Date.Equals(modificationDate.Date));
                        break;
                    case string key when nameof(EntityBase.Active).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        bool.TryParse(f.Value, out var active);
                        query = query.Where(x => x.Active.Equals(active));
                        break;
                    default:
                        var entityType = query.ElementType;
                        var propertyExists = entityType.GetProperty(f.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyExists == null)
                            throw new DomainException(nameof(ServiceBase<TEntity>), nameof(Search), $"There is no property named \"{f.Key}\" on {entityType.Name}");
                        break;
                }
            }

            var result = new PagedResult<TEntity>
            {
                CurrentPage = filter.Page,
                PageSize = filter.Limit,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / filter.Limit;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (filter.Page - 1) * filter.Limit;
            result.Result = query.Skip(skip).Take(filter.Limit).ToList();

            return result;
        }

        /// <summary>
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public virtual TEntity Update(TEntity entity)
        {
            entity.UpdateEntity(_userHelper.LoggedUser.Id);

            if (!entity.IsValid())
                throw new DomainException(nameof(ServiceBase<TEntity>), nameof(Add), "Invalid entity");

            _repository.Update(entity);

            _repository.SaveChanges();

            return entity;
        }

        #endregion
    }
}