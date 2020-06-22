using Sample.Domain.Entities;
using Sample.Domain.Exceptions;
using Sample.Domain.Interfaces;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Sample.Service
{
    public class UserService : ServiceBase<User>, IUserService
    {
        #region [ Properties ]

        private readonly IUserRepository _userRepository;

        #endregion

        #region [ Constructor ]

        public UserService(IUserRepository repository, IUserHelper userHelper) : base(repository, userHelper)
        {
            _userRepository = repository;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Add a new user to the database
        /// </summary>
        /// <param name="entity">User</param>
        /// <returns>User</returns>
        public override User Add(User entity)
        {
            if (GetByLogin(entity.Login) != null)
            {
                throw new DomainException(nameof(UserService), nameof(Add), "Login already exists");
            }

            return base.Add(entity);
        }

        /// <summary>
        /// Update user on the database
        /// </summary>
        /// <param name="entity">User</param>
        /// <returns>User</returns>
        public override User Update(User entity)
        {
            var loginUser = GetByLogin(entity.Login);

            if (loginUser != null && loginUser.Id != entity.Id)
            {
                throw new DomainException(nameof(UserService), nameof(Update), "Login is in use by another user");
            }

            return base.Update(entity);
        }

        /// <summary>
        /// Searches a list of users based on a query filter
        /// </summary>
        /// <remarks><![CDATA[
        /// As EntityFramework does not allow reflection to be used, filters on properties must be done manually. Therefore, ServiceBase 
        /// implements the basic filters in the EntityBase properties. This method will implement the filters in the user's own properties,
        /// and after that will call the service base to filter the inherit properties from EntityBase.
        /// ]]>
        /// </remarks>
        /// <param name="filter">Query Filter</param>
        /// <param name="query">Query (see the method remarks for more info)</param>
        /// <returns>List of Users</returns>
        public override PagedResult<User> Search(QueryFilter filter, IQueryable<User> query = null)
        {
            query = _repository.GetAll();

            foreach (var f in filter.Filters)
            {
                switch (f.Key)
                {
                    case string key when nameof(User.Name).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        query = query.Where(x => x.Name.StartsWith(f.Value, StringComparison.OrdinalIgnoreCase));
                        break;
                    case string key when nameof(User.Login).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        query = query.Where(x => x.Login.StartsWith(f.Value, StringComparison.OrdinalIgnoreCase));
                        break;
                    case string key when nameof(User.TypeId).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        int.TryParse(f.Value, out var typeId);
                        query = query.Where(x => x.TypeId.Equals((EnumUserType)typeId) || x.TypeId.ToString().Equals(f.Value, StringComparison.OrdinalIgnoreCase));
                        break;
                    case string key when nameof(User.Password).Equals(key, StringComparison.InvariantCultureIgnoreCase):
                        throw new DomainException(nameof(UserService), nameof(Search), "You cannot search users based on its passwords");
                    // No default statement was placed because the inherited EntityBase properties will be handled by ServiceBase
                }
            }

            return base.Search(filter, query);
        }

        /// <summary>
        /// Returns a user based on its login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>User</returns>
        public User GetByLogin(string login)
        {
            return _userRepository.GetByLogin(login);
        }

        #endregion
    }
}