using Sample.Domain.Entities;

namespace Sample.Domain.Interfaces.Services
{
    public interface IUserService : IServiceBase<User>
    {
        /// <summary>
        /// Returns a user based on its login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>User</returns>
        User GetByLogin(string login);
    }
}