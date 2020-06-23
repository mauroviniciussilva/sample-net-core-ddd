using Sample.Domain.Entities.Helpers;
using Sample.Domain.Exceptions;
using Sample.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Sample.Application.Helpers
{
    public class UserHelper : IUserHelper
    {
        #region [ Private Properties ]

        private readonly IHttpContextAccessor _context;
        private UserIdentity _loggedUser;

        #endregion

        #region [ Public Properties ]

        public UserIdentity LoggedUser
        {
            get
            {
                if (_loggedUser != null)
                    return _loggedUser;

                ClaimsPrincipal user = _context.HttpContext.User;
                var id = user.Claims.FirstOrDefault(c => c.Type == "Id");
                var name = user.Claims.FirstOrDefault(c => c.Type == "Name");


                if (id == null || name == null)
                    throw new ExpiredUserException("User needs to re-login.");

                _loggedUser = new UserIdentity(int.Parse(id.Value), name.Value);

                return _loggedUser;
            }
        }

        #endregion

        #region [ Constructor ]

        public UserHelper(IHttpContextAccessor context) => _context = context;

        #endregion        
    }
}