using Sample.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sample.Application.ViewModel
{
    /// <summary>
    /// A ViewModel that represents the User entity when creating or updating an user through API
    /// </summary>
    public class UserEditViewModel
    {
        /// <summary>
        /// Identity of the User
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// User login
        /// </summary>
        [Required]
        public string Login { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Type of the user
        /// </summary>
        [Required]
        public int TypeId { get; set; }
        /// <summary>
        /// Indicate if the user is active or not
        /// </summary>
        [Required]
        public bool Active { get; set; }
    }

    /// <summary>
    /// A ViewModel that represents the User entity when listing users through API
    /// </summary>
    public class UserListViewModel
    {
        /// <summary>
        /// Identity of the User
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Type of the user
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// Indicate if the user is active or not
        /// </summary>
        public bool Active { get; set; }
    }

    /// <summary>
    /// A ViewModel that represents the User entity when logging in the system
    /// </summary>
    public class UserLoginViewModel
    {
        /// <summary>
        /// User login
        /// </summary>
        [Required]
        public string Login { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// A ViewModel that contains the login api return data
    /// </summary>
    public class UserTokenViewModel
    {
        /// <summary>
        /// Identity of the User
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User authorization token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// User login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Type of the user
        /// </summary>
        public EnumUserType TypeId { get; set; }
    }

    /// <summary>
    /// The ViewModel that is returned when the login fails
    /// </summary>
    public class UserLoginErrorViewModel
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
    }
}