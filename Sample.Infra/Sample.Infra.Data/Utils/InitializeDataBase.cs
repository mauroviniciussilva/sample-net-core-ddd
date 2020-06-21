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
                    new User("Administrator", "admin", "admin", EnumUserType.Administrator, 1),
                    new User("Editor", "editor", "editor", EnumUserType.Editor, 1),
                    new User("General", "general", "general", EnumUserType.General, 1)
                };

                context.AddRange(usuarios);
                context.SaveChanges();
            }
        }
    }
}
