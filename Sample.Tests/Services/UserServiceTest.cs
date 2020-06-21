using Bogus;
using Moq;
using Sample.Domain.Entities;
using Sample.Domain.Entities.Helpers;
using Sample.Domain.Exceptions;
using Sample.Domain.Interfaces;
using Sample.Domain.Interfaces.Repositories;
using Sample.Domain.Interfaces.Services;
using Sample.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sample.Tests.Services
{
    public class UserServiceTest
    {
        #region [ Properties ]

        const int _loggerUserId = 1;
        private readonly List<User> _users;
        private readonly IUserService _userService;

        #endregion

        #region [ Constructor ]

        public UserServiceTest()
        {
            _users = GenerateUserList();
            _userService = CreateUserService();
        }

        #endregion

        #region [ Test Methods ]

        /// <summary>
        /// Tests the logic of adding user to the database
        /// </summary>
        /// <remarks><![CDATA[
        ///     Although this is an existing method in the ServiceBase, it has been overridden in the UserService
        /// ]]>
        /// </remarks>
        /// <param name="name">User Name</param>
        [Theory]
        [InlineData("Test User")]
        [InlineData(null)]
        public void Add_User(string name)
        {
            var userLogin = "testuser";
            var currentDateTime = DateTime.Now;
            var user = new User(name, userLogin, "password", EnumUserType.Administrator, _loggerUserId);

            if (name == null)
            {
                // Should return an error if the IsValid() method finds any inconsistencies
                Assert.Throws<DomainException>(() =>
                {
                    _userService.Add(user);
                });
            }
            else
            {
                var newUser = _userService.Add(user);

                Assert.NotNull(newUser);
                // Checking if the Service is setting the parameters as it should
                Assert.Equal(_loggerUserId, newUser.UserCreationId);
                Assert.True(currentDateTime <= newUser.CreationDate);
                Assert.True(newUser.Active);
                // Must return an error when trying to add another user with the same login
                Assert.Throws<DomainException>(() =>
                {
                    var duplicatedUser = new User("Duplicated Login", userLogin, "password", EnumUserType.Administrator, _loggerUserId);
                    _userService.Add(duplicatedUser);
                });
            }
        }

        /// <summary>
        /// Tests the logic of getting an user based on its login
        /// </summary>
        [Fact]
        public void Get_By_Login()
        {
            var firstUser = _users.FirstOrDefault();

            var user = _userService.GetByLogin(firstUser.Login);

            Assert.NotNull(user);
            Assert.Equal(firstUser, user);
        }

        /// <summary>
        /// Update a existing user on the database
        /// </summary>
        /// <remarks><![CDATA[
        ///     Although this is an existing method in the ServiceBase, it has been overridden in the UserService
        /// ]]>
        /// </remarks>
        /// <param name="login"></param>
        [Theory]
        [InlineData("LoginInUse")]
        [InlineData(null)]
        [InlineData("newlogin")]
        public void Update_User(string login)
        {
            // Arrange
            var currentDateTime = DateTime.Now;
            var user = _users.FirstOrDefault();

            if (login == "LoginInUse")
            {
                // Should return an error if the IsValid() method finds any inconsistencies
                Assert.Throws<DomainException>(() =>
                {
                    var lastUser = _users.LastOrDefault();
                    lastUser.Login = user.Login;
                    _userService.Update(lastUser);
                });
            }
            if (login == null)
            {
                // Should return an error if the IsValid() method finds any inconsistencies
                Assert.Throws<DomainException>(() =>
                {
                    user.Login = login;
                    _userService.Update(user);
                });
            }
            else
            {
                user.Login = login;

                var updatedUser = _userService.Update(user);

                // Checking if the Service is setting the parameters as it should
                Assert.Equal(_loggerUserId, updatedUser.UserModificationId);
                Assert.Equal(login, updatedUser.Login);
                Assert.True(currentDateTime <= updatedUser.ModificationDate);
            }
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Generate a mock repository and a mock user helper for the current test execution
        /// </summary>
        /// <returns></returns>
        private IUserService CreateUserService()
        {
            #region [ Mock of UserRepository ]

            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(us => us.GetAllActive())
                .Returns(() =>
                {
                    return _users.Where(u => u.Active).AsQueryable();
                });

            userRepository.Setup(us => us.GetById(It.IsAny<int>()))
                .Returns<int>((id) =>
                {
                    return _users.FirstOrDefault(u => u.Id == id && u.Active);
                });

            userRepository.Setup(ur => ur.Add(It.IsAny<User>()))
                .Returns<User>((usuario) =>
                {
                    _users.Add(usuario);
                    return usuario;
                });

            userRepository.Setup(ur => ur.GetByLogin(It.IsAny<string>()))
                .Returns<string>((login) =>
                {
                    return _users.FirstOrDefault(u => u.Login == login && u.Active);
                });

            #endregion

            #region [ Mock of UserHelper ]

            var userHelper = new Mock<IUserHelper>();

            userHelper.Setup(uh => uh.LoggedUser).Returns(() => new UserIdentity(_loggerUserId, "Admin"));

            #endregion

            return new UserService(userRepository.Object, userHelper.Object);
        }

        /// <summary>
        /// Generate a list with 10 fake users
        /// </summary>
        /// <returns>List of Users</returns>
        private List<User> GenerateUserList()
        {
            var fakeUsers = new Faker<User>(locale: "en_US").StrictMode(false)
                .RuleFor(u => u.Name, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.Login, (f, u) => u.Name.ToLower())
                .RuleFor(u => u.Password, (f, u) => f.Random.AlphaNumeric(6).ToString())
                .RuleFor(u => u.TypeId, f => f.PickRandom<EnumUserType>())
                .RuleFor(u => u.UserCreationId, 1)
                .RuleFor(u => u.Id, f => (f.IndexFaker + 1))
                .RuleFor(u => u.Active, true)
                .RuleFor(u => u.CreationDate, DateTime.Now);

            return fakeUsers.Generate(10);
        }

        #endregion
    }
}