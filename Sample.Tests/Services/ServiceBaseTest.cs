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
    public class ServiceBaseTest
    {
        #region [ Properties ]

        const int _loggerUserId = 1;
        private readonly List<User> _users;
        private readonly IServiceBase<User> _genericUserService;

        #endregion

        #region [ Constructor ]

        public ServiceBaseTest()
        {
            _users = GenerateEntitiesList();
            _genericUserService = CreateBaseService();
        }

        #endregion

        #region [ Test Methods ]

        /// <summary>
        /// Tests the logic of adding an entity to the database
        /// </summary>
        /// <param name="name">User Name</param>
        [Theory]
        [InlineData("Test User")]
        [InlineData(null)]
        public void Add_Entity(string name)
        {
            var currentDateTime = DateTime.Now;
            var entity = new User(name, "testuser", "password", EnumUserType.Administrator, 0);

            if (name == null)
            {
                // Should return an error if the IsValid() method finds any inconsistencies
                Assert.Throws<DomainException>(() =>
                {
                    _genericUserService.Add(entity);
                });
            }
            else
            {
                var newEntity = _genericUserService.Add(entity);

                Assert.NotNull(newEntity);
                // Checking if the Service is setting the parameters as it should
                Assert.Equal(_loggerUserId, newEntity.UserCreationId);
            }
        }

        /// <summary>
        /// Teste the logic of deleting an entity based on its id
        /// </summary>
        /// <param name="id">Entity's Id</param>
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(999999)]
        public void Delete_By_Id(int id)
        {
            if (id == 0)
            {
                // Should return an error if the id passed is 0
                Assert.Throws<DomainException>(() =>
                {
                    _genericUserService.DeleteById(id);
                });
            }
            else if (id == 1)
            {
                var user = _genericUserService.GetById(id);

                _genericUserService.DeleteById(id);

                // This method only inactivates the record, but database queries return only active records. 
                // This was designed in this way to recover a record deleted by accident, for data analysis, 
                // among other possibilities. 
                Assert.Null(_genericUserService.GetById(id));
                Assert.Null(_genericUserService.GetAll().FirstOrDefault(x => x.Id == id));
            }
            else
            {
                // Should return an error if the id passed is not found
                Assert.Throws<DomainException>(() =>
                {
                    _genericUserService.DeleteById(id);
                });
            }
        }

        /// <summary>
        /// Tests the logic of getting all the entites from the database
        /// </remarks>
        [Fact]
        public void Get_All()
        {
            var entity = _genericUserService.GetAll();

            Assert.NotNull(entity);
        }

        /// <summary>
        /// Tests the logic of getting an entity based on its Id
        /// </summary>
        [Fact]
        public void Get_By_Id()
        {
            var firstEntity = _users.FirstOrDefault();

            var entity = _genericUserService.GetById(firstEntity.Id);

            Assert.NotNull(entity);
            Assert.Equal(firstEntity, entity);
        }

        /// <summary>
        /// Tests the logic of updating a existing entity on the database
        /// </summary>
        /// <param name="name">User Name</param>
        [Theory]
        [InlineData(null)]
        [InlineData("New Name")]
        public void Update_Entity(string name)
        {
            var currentDateTime = DateTime.Now;
            var entity = _users.FirstOrDefault();

            if (name == null)
            {
                // Should return an error if the IsValid() method finds any inconsistencies
                Assert.Throws<DomainException>(() =>
                {
                    entity.Name = name;
                    _genericUserService.Update(entity);
                });
            }
            else
            {
                entity.Name = name;

                var updatedEntity = _genericUserService.Update(entity);

                // Checking if the ServiceBase is setting the parameters as it should
                Assert.Equal(_loggerUserId, updatedEntity.UserModificationId);
                Assert.Equal(name, updatedEntity.Name);
                Assert.True(currentDateTime <= updatedEntity.ModificationDate);
            }
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Generate a mock repository and a mock user helper for the current test execution
        /// </summary>
        /// <returns>User Service Base</returns>
        private IServiceBase<User> CreateBaseService()
        {
            #region [ Mock of RepositoryBase ]

            var baseUserRepository = new Mock<IRepositoryBase<User>>();

            baseUserRepository.Setup(us => us.GetAllActive())
                .Returns(() =>
                {
                    return _users.Where(u => u.Active).AsQueryable();
                });

            baseUserRepository.Setup(us => us.GetById(It.IsAny<int>()))
                .Returns<int>((id) =>
                {
                    return _users.FirstOrDefault(u => u.Id == id && u.Active);
                });

            baseUserRepository.Setup(ur => ur.Add(It.IsAny<User>()))
                .Returns<User>((user) =>
                {
                    _users.Add(user);
                    return user;
                });

            #endregion

            #region [ Mock of UserHelper ]

            var userHelper = new Mock<IUserHelper>();

            userHelper.Setup(uh => uh.LoggedUser).Returns(() => new UserIdentity(_loggerUserId, "Admin"));

            #endregion

            return new ServiceBase<User>(baseUserRepository.Object, userHelper.Object);
        }

        /// <summary>
        /// Generate a list with 10 fake entities
        /// </summary>
        /// <returns>List of Users</returns>
        private List<User> GenerateEntitiesList()
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