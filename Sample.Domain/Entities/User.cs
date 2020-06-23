using Sample.Domain.Exceptions;
using System;
using System.Linq;

namespace Sample.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public EnumUserType TypeId { get; set; }

        public User()
        {
            // Required for EntityFramework
        }

        public User(string name, string login, string password, EnumUserType userType, int userCreationId)
        {
            Name = name;
            Login = login;
            Password = password;
            UserCreationId = userCreationId;
            TypeId = userType;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                AddError("Name is required!");
            }

            if (string.IsNullOrEmpty(Login))
            {
                AddError("Login is required!");
            }

            if (string.IsNullOrEmpty(Password))
            {
                AddError("Password is required!");
            }

            if (CreationDate == DateTime.MinValue)
            {
                AddError("Creation date not defined");
            }

            if (Errors.Any())
            {
                throw new DomainException(Errors.Select(e => e.Error));
            }
        }
    }

    public enum EnumUserType
    {
        Administrator = 1,
        Editor = 2,
        General = 3
    }
}