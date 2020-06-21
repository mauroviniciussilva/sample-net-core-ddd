using Sample.Domain.Entities;

namespace Sample.Application.ViewModel
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public int TypeId { get; set; }
    }

    public class UserListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public int TypeId { get; set; }
        public bool Active { get; set; }
    }

    public class UserLoginViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UserTokenViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Login { get; set; }
        public EnumUserType TypeId { get; set; }
    }
}