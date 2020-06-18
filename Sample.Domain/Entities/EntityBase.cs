using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.Entities
{
    public abstract class EntityBase
    {
        [NotMapped]
        internal List<DomainError> Erros { get; set; }

        protected EntityBase()
        {
            Erros = new List<DomainError>();
        }

        /// <summary>
        /// Primary key of the entity
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the user that created the registry
        /// </summary>
        public int UserCreationId { get; set; }
        /// <summary>
        /// Creation date of the entity
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Id of the user who last modified the user
        /// </summary>
        public int? UserModificationId { get; set; }
        /// <summary>
        /// Last date the record was modified
        /// </summary>
        public DateTime? ModificationDate { get; set; }
        /// <summary>
        /// Boolean that indicates whether the record is active or not
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Method que vali
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        /// <summary>
        /// Method that checks whether the entity is valid or not
        /// </summary>
        /// <param name="error"></param>
        public void AddError(string error)
        {
            Erros.Add(new DomainError(error));
        }       

        /// <summary>
        /// Method that returns the erros of the entity
        /// </summary>
        /// <returns></returns>
        public List<DomainError> GetErros() => Erros;
    }

    public class DomainError
    {
        public string Erro { get; set; }

        public DomainError(string erro)
        {
            Erro = erro;
        }
    }
}
