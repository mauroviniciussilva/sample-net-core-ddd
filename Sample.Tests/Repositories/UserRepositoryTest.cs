using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Repositories;
using Sample.Infra.Data.Context;
using Sample.Infra.Data.Repository;
using System;
using Xunit;

namespace Sample.Tests.Repositories
{
    public class UserRepositoryTest
    {
        #region [ Properties ]

        private readonly IUserRepository _userRepository;

        #endregion

        #region [ Constructor ]

        public UserRepositoryTest()
        {
            _userRepository = CreateUserRepository();
        }

        #endregion

        #region [ Test Methods ]

        /// <summary>
        /// Tests if the method for return a user by login is working properly
        /// </summary>
        [Fact]
        public void Get_User_By_Login()
        {
            var user = _userRepository.GetByLogin("admin");

            Assert.Equal(1, user.Id);
            Assert.Equal("Administrator", user.Name);
            Assert.Equal("admin", user.Login);
            Assert.Equal("admin", user.Password);
            Assert.Equal(EnumUserType.Administrator, user.TypeId);
            Assert.Equal(1, user.UserCreationId);
            Assert.True(user.CreationDate < DateTime.Now);
            Assert.True(user.Active);
        }

        #endregion

        #region [ Private Methods ]

        private IUserRepository CreateUserRepository()
        {
            // Creating a DataBase in Memory
            var options = new DbContextOptionsBuilder<CoreContext>().UseInMemoryDatabase(databaseName: "UserRepository").Options;
            // The constructor will seed the fake database with the default users, so no seed is needed here
            var context = new CoreContext(options);
            // Creating the repository
            var userRepository = new UserRepository(context);

            return userRepository;
        }

        #endregion
    }
}
