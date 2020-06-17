using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Helpers;
using Sample.Api.ViewModel;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Services;
using System;

namespace Sample.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase<User, UserEditViewModel, UserListViewModel>
    {
        #region [ Properties ]

        private readonly IUserService _userService;

        #endregion

        #region [ Constructor ]

        public UserController(IUserService service, IMapper mapper) : base(service, mapper)
        {
            _userService = service;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Login the user in the system
        /// </summary>
        /// <param name="loginViewModel">Login information</param>
        /// <returns>Token</returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody]UserLoginViewModel loginViewModel)
        {
            try
            {
                var user = _userService.GetByLogin(loginViewModel.Login);

                if (user == null)
                    return BadRequest(new { message = "User not found" });

                if (user.Password != loginViewModel.Password)
                    return BadRequest(new { message = "Invalid password" });

                var token = TokenService.GenerateToken(user.Id, user.Name, "admin");

                var tokenViewModel = new UserTokenViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Token = token,
                    Login = user.Login,
                    TypeId = user.TypeId
                };

                return Ok(tokenViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion
    }
}