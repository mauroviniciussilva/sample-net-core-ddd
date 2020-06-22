using Microsoft.EntityFrameworkCore;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Repositories;
using Sample.Infra.Data.Context;
using Sample.Infra.Data.Repositories;
using System;
using System.Linq;
using Xunit;

namespace Sample.Tests.Repositories
{
    public class RepositoryBaseTest
    {
        #region [ Properties ]

        const int _loggerUserId = 1;
        private readonly IRepositoryBase<User> _userRepository;

        #endregion

        #region [ Constructor ]

        public RepositoryBaseTest()
        {
            _userRepository = CreateRepository();
        }

        #endregion

        #region [ Test Methods ]

        /// <summary>
        /// Tests if the method for return all the entities is working properly
        /// </summary>
        [Fact]
        public void Get_All_Active_Users()
        {
            // Taking 3 prevents an error from being thrown when the test 'Add_User_And_Save_Changes' 
            // runs before it, because that test changes the context collection
            var users = _userRepository.GetAllActive().Take(3);

            Assert.Collection(users,
                user =>
                {
                    Assert.Equal(1, user.Id);
                    Assert.Equal("Administrator", user.Name);
                    Assert.Equal("admin", user.Login);
                    Assert.Equal("admin", user.Password);
                    Assert.Equal(EnumUserType.Administrator, user.TypeId);
                    Assert.Equal(1, user.UserCreationId);
                    Assert.True(user.CreationDate < DateTime.Now);
                    Assert.True(user.Active);
                },
                user =>
                {
                    Assert.Equal(2, user.Id);
                    Assert.Equal("Editor", user.Name);
                    Assert.Equal("editor", user.Login);
                    Assert.Equal("editor", user.Password);
                    Assert.Equal(EnumUserType.Editor, user.TypeId);
                    Assert.Equal(1, user.UserCreationId);
                    Assert.True(user.CreationDate < DateTime.Now);
                    Assert.True(user.Active);
                },
                user =>
                {
                    Assert.Equal(3, user.Id);
                    Assert.Equal("General", user.Name);
                    Assert.Equal("general", user.Login);
                    Assert.Equal("general", user.Password);
                    Assert.Equal(EnumUserType.General, user.TypeId);
                    Assert.Equal(1, user.UserCreationId);
                    Assert.True(user.CreationDate < DateTime.Now);
                    Assert.True(user.Active);
                }
            );
        }

        /// <summary>
        /// Tests if the method for return a record by id is working properly
        /// </summary>
        [Fact]
        public void Get_By_Id()
        {
            var user = _userRepository.GetById(1);

            Assert.Equal(1, user.Id);
            Assert.Equal("Administrator", user.Name);
            Assert.Equal("admin", user.Login);
            Assert.Equal("admin", user.Password);
            Assert.Equal(EnumUserType.Administrator, user.TypeId);
            Assert.Equal(1, user.UserCreationId);
            Assert.True(user.CreationDate < DateTime.Now);
            Assert.True(user.Active);
        }

        /// <summary>
        /// Tests if the method for adding new records and the method for saving changes are working properly
        /// </summary>
        [Fact]
        public void Add_User_And_Save_Changes()
        {
            var userName = "Test User";
            var userLogin = "test";
            var userPassword = "test";
            var userType = EnumUserType.Administrator;

            var user = _userRepository.Add(new User(userName, userLogin, userPassword, userType, _loggerUserId));

            Assert.True(user.Id > 0);
            Assert.Equal(userName, user.Name);
            Assert.Equal(userLogin, user.Login);
            Assert.Equal(userPassword, user.Password);
            Assert.Equal(userType, user.TypeId);
            Assert.Equal(_loggerUserId, user.UserCreationId);
            Assert.True(user.CreationDate < DateTime.Now);
            Assert.True(user.Active);

            _userRepository.SaveChanges();

            var savedUser = _userRepository.GetById(user.Id);
            Assert.True(savedUser.Id > 0);
            Assert.Equal(userName, savedUser.Name);
            Assert.Equal(userLogin, savedUser.Login);
            Assert.Equal(userPassword, savedUser.Password);
            Assert.Equal(userType, savedUser.TypeId);
            Assert.Equal(_loggerUserId, user.UserCreationId);
            Assert.True(savedUser.CreationDate < DateTime.Now);
            Assert.True(savedUser.Active);
        }

        /// <summary>
        /// Tests if the method for updating existing records is working properly
        /// </summary>
        /// <remarks><![CDATA[
        /// TODO: Fix the individual execution of this test
        /// This test doesn't work when you run it individually, but it works normally when you run all the tests. The 
        /// following error is thrown:
        ///     System.InvalidOperationException : The instance of entity type 'User' cannot be tracked because another 
        ///     instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, 
        ///     ensure that only one entity instance with a given key value is attached. 
        ///     Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.
        /// ]]></remarks>
        [Fact]
        public void Update_User()
        {
            var userName = "Updated General";
            var userLogin = "updatedgeneral";
            var userPassword = "updatedpassword";
            var userType = EnumUserType.Editor;

            var user = _userRepository.GetById(3);
            user.Name = userName;
            user.Login = userLogin;
            user.Password = userPassword;
            user.TypeId = userType;
            user.UpdateEntity(_loggerUserId);

            _userRepository.Update(user);

            Assert.Equal(3, user.Id);
            Assert.Equal(userName, user.Name);
            Assert.Equal(userLogin, user.Login);
            Assert.Equal(userPassword, user.Password);
            Assert.Equal(userType, user.TypeId);
            Assert.Equal(_loggerUserId, user.UserModificationId);
            Assert.True(user.ModificationDate < DateTime.Now);
            Assert.True(user.Active);
        }

        #endregion

        #region [ Private Methods ]

        private IRepositoryBase<User> CreateRepository()
        {
            // Creating a DataBase in Memory
            var options = new DbContextOptionsBuilder<CoreContext>().UseInMemoryDatabase(databaseName: "RepositoryBase").Options;
            // The constructor will seed the fake database with the default users, so no seed is needed here
            var context = new CoreContext(options);
            // Creating the repository
            var userRepository = new RepositoryBase<User>(context);

            return userRepository;
        }

        #endregion
    }
}
