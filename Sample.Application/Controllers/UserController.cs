using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Helpers;
using Sample.Application.ViewModel;
using Sample.Domain.Entities;
using Sample.Domain.Interfaces.Services;
using System;

namespace Sample.Application.Controllers
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
        /// <response code="200">Returns token and user information</response>
        /// <response code="400">If login fails</response>        
        [Produces("application/json")]
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginErrorViewModel), StatusCodes.Status400BadRequest)]
        public IActionResult Authenticate([FromBody]UserLoginViewModel loginViewModel)
        {
            try
            {
                var user = _userService.GetByLogin(loginViewModel.Login);

                if (user == null)
                    return BadRequest(new UserLoginErrorViewModel { Message = "User not found" });

                if (user.Password != loginViewModel.Password)
                    return BadRequest(new UserLoginErrorViewModel { Message = "Invalid password" });

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
                return BadRequest(new UserLoginErrorViewModel() { Message = ex.Message });
            }
        }

        #endregion
    }
}