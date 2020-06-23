using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.Entities
{
    public abstract class EntityBase
    {
        /// <summary>
        /// Primary key of the entity
        /// </summary>
        public int Id { get; protected set; }
        /// <summary>
        /// Id of the user that created the registry
        /// </summary>
        public int UserCreationId { get; protected set; }
        /// <summary>
        /// Creation date of the entity
        /// </summary>
        public DateTime CreationDate { get; protected set; }
        /// <summary>
        /// Id of the user who last modified the user
        /// </summary>
        public int? UserModificationId { get; protected set; }
        /// <summary>
        /// Last date the record was modified
        /// </summary>
        public DateTime? ModificationDate { get; protected set; }
        /// <summary>
        /// Boolean that indicates whether the record is active or not
        /// </summary>
        public bool Active { get; protected set; }

        /// <summary>
        /// List of domain errors
        /// </summary>
        [NotMapped]
        internal List<DomainError> Errors { get; set; }

        #region [ Constructor ]

        protected EntityBase()
        {
            Errors = new List<DomainError>();
            CreationDate = DateTime.Now;
            Active = true;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <returns></returns>
        public abstract void Validate();

        /// <summary>
        /// Adds a domain error to the entity
        /// </summary>
        /// <param name="error">Message error</param>
        public void AddError(string error)
        {
            Errors.Add(new DomainError(error));
        }

        /// <summary>
        /// Returns the domain erros of the entity.
        /// </summary>
        /// <returns></returns>
        public List<DomainError> GetErros() => Errors;

        /// <summary>
        /// Changes modification information on the entity.
        /// </summary>
        /// <param name="userModificationId">Id of the Logged User</param>
        public virtual void UpdateEntity(int userModificationId)
        {
            ModificationDate = DateTime.Now;
            UserModificationId = userModificationId;
        }

        /// <summary>
        /// Changes the cration user of the entity. This method should only be used when the 
        /// entity construction is done by EntityFramework.
        /// </summary>
        /// <param name="userCreationId">Id of the creation user</param>
        public void UpdateUserCreationId(int userCreationId)
        {
            UserCreationId = userCreationId;
        }

        /// <summary>
        /// Restore the entity to a state of activity.
        /// </summary>
        /// <param name="userModificationId"></param>
        public void Reactivate(int userModificationId)
        {
            UpdateEntity(userModificationId);
            Active = true;
        }

        /// <summary>
        /// Make the entity inactive.
        /// </summary>
        /// <param name="userModificationId"></param>
        public void Inactivate(int userModificationId)
        {
            UpdateEntity(userModificationId);
            Active = false;
        }

        public string GetPropValue(string propName)
        {
            var value = string.Empty;

            var prop = GetType().GetProperty(propName);
            if (prop != null) 
                value = prop.GetValue(this) == null? string.Empty : prop.GetValue(this).ToString();

            return value;
        }

        #endregion
    }

    /// <summary>
    /// Entity domain error
    /// </summary>
    public class DomainError
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; set; }

        public DomainError(string error)
        {
            Error = error;
        }
    }
}
