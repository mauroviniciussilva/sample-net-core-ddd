using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Repositories;
using Sample.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Sample.Infra.Data.Repositories
{
    /// <summary>
    /// A generic class of repository that contains basic methods of a CRUD and manipulation of the registries from the database
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : EntityBase
    {
        #region [ Properties ]

        protected readonly CoreContext Context;
        protected readonly DbSet<TEntity> DbSet;

        #endregion

        #region [ Constructor ]

        public RepositoryBase(CoreContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
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
            DbSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// Inactive an entity based on its id
        /// </summary>
        /// <param name="id">Entity's Id</param>
        /// <param name="loggedUserId">Logged In User Id</param>
        public void Inactivate(int id, int loggedUserId)
        {
            TEntity entity = GetById(id);
            entity.Active = false;
            entity.UserModificationId = loggedUserId;
            entity.ModificationDate = DateTime.Now;
            DbSet.Update(entity);
        }

        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().Where(x => x.Active);
        }

        /// <summary>
        /// Returns an entity based on its id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(int id)
        {
            return Context.Set<TEntity>().FirstOrDefault(x => x.Id == id && x.Active);
        }

        /// <summary>
        /// Returns an entity and its childs based on its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public virtual TEntity GetByIdWithInclude(int id, string include)
        {
            return Context.Set<TEntity>().Include(include).FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Commit the changes to the database
        /// </summary>
        /// <remarks><![CDATA[
        /// You need to use that method to the changes have some effect in the database
        /// ]]>
        /// </remarks>
        /// <returns></returns>
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// Searches a list of entities based on a query filter
        /// </summary>
        /// <param name="filter">Query Filter</param>
        /// <returns>List of Entities</returns>
        public virtual PagedResult<TEntity> Search(QueryFilter filter)
        {
            filter.Filters.TryGetValue("Page", out string page);
            filter.Filters.TryGetValue("Limit", out string limit);

            var pagina = 1;
            var limite = 20;

            if (!string.IsNullOrEmpty(page)) pagina = int.Parse(page);
            if (!string.IsNullOrEmpty(limit)) limite = int.Parse(limit);

            IQueryable<TEntity> query = GetAll();

            var result = new PagedResult<TEntity>
            {
                CurrentPage = pagina,
                PageSize = limite,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / limite;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (pagina - 1) * limite;
            result.Result = query.Skip(skip)
                                  .Take(limite)
                                  .ToList();
            return result;
        }

        /// <summary>
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public virtual TEntity Update(TEntity entity)
        {
            var result = DbSet.Attach(entity);
            result.State = EntityState.Modified;
            result.Property(x => x.UserCreationId).IsModified = false;
            result.Property(x => x.CreationDate).IsModified = false;

            return entity;
        }

        #endregion
    }
}