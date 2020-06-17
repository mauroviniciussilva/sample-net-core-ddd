using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Application;
using Sample.Domain.Interfaces.Services;

namespace Sample.Application
{
    public class UserApplication : ApplicationBase<User>, IUserApplication
    {
        #region [ Properties ]

        private readonly IUserService _userservice;

        #endregion

        #region [ Constructor ]

        public UserApplication(IUserService service) : base(service)
        {
            _userservice = service;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns a user based on its login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>User</returns>
        public User GetByLogin(string login)
        {
            return _userservice.GetByLogin(login);
        }

        #endregion
    }
}
