using Sample.Domain.Entities;

namespace Sample.Domain.Interfaces.Application
{
    public interface IUserApplication : IApplicationBase<User>
    {
        /// <summary>
        /// Returns a user based on its login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>User</returns>
        User GetByLogin(string login);
    }
}
