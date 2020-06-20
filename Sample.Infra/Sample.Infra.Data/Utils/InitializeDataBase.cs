using Sample.Domain.Entities;
using Sample.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Infra.Data.Utils
{
    /// <summary>
    /// This class is used to seed the database when there are no records
    /// </summary>
    public class InitializeDataBase
    {
        public static void Initialize(CoreContext context)
        {
            context.Database.EnsureCreated();

            if (!context.User.Any())
            {
                var usuarios = new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Name = "Administrator",
                        Login = "admin",
                        Password = "admin",
                        TypeId = EnumUserType.Administrator,
                        UserCreationId = 1,
                        CreationDate = DateTime.Now,
                        Active = true
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Editor",
                        Login = "editor",
                        Password = "editor",
                        TypeId = EnumUserType.Editor,
                        UserCreationId = 1,
                        CreationDate = DateTime.Now,
                        Active = true
                    },
                    new User
                    {
                        Id = 3,
                        Name = "General",
                        Login = "general",
                        Password = "general",
                        TypeId = EnumUserType.General,
                        UserCreationId = 1,
                        CreationDate = DateTime.Now,
                        Active = true
                    }
                };

                context.AddRange(usuarios);
                context.SaveChanges();
            }
        }
    }
}
