using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Repositories;
using Sample.Infra.Data.Context;
using Sample.Infra.Data.Repositories;
using System.Linq;

namespace Sample.Infra.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(CoreContext context) : base(context) { }

        /// <summary>
        /// Returns a user based on its login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>User</returns>
        public User GetByLogin(string login)
        {
            return DbSet.FirstOrDefault(p => p.Login == login);
        }
    }
}