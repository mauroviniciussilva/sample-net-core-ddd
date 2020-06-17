using Sample.Domain.Entities.Helpers;

namespace Sample.Domain.Interfaces
{
    public interface IUserHelper
    {
        /// <summary>
        /// Information about the current logged user
        /// </summary>
        UserIdentity LoggedUser { get; }
    }
}