using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Repositories;
using Sample.Infra.Data.Context;
using System.Linq;

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
        /// Returns all the active registries from the database
        /// </summary>
        /// <returns>List of Entities</returns>
        public virtual IQueryable<TEntity> GetAllActive()
        {
            return Context.Set<TEntity>().Where(x => x.Active);
        }

        /// <summary>
        /// Returns all the registries from the database
        /// </summary>
        /// <remarks><![CDATA[
        /// This method should be called only when you intend to return inactive records
        /// ]]>
        /// </remarks>
        /// <returns>List of Entities</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>();
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
        /// Update a existing entity on the database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public virtual TEntity Update(TEntity entity)
        {
            var result = DbSet.Attach(entity);
            result.State = EntityState.Modified;
            result.Property(x => x.Id).IsModified = false;
            result.Property(x => x.UserCreationId).IsModified = false;
            result.Property(x => x.CreationDate).IsModified = false;

            return entity;
        }

        #endregion
    }
}