namespace Sample.Domain.Entities
{
    public enum EnumUserType
    {
        Administrator = 1,
        Editor = 2,
        General = 3
    }

    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public EnumUserType TypeId { get; set; }

        public User()
        {

        }

        public User(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }

        public override bool IsValid()
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

            return Erros.Count == 0;
        }
    }
}