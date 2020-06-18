using Sample.Domain.Entities.Helpers;
using Sample.Domain.Interfaces;

namespace Sample.Background
{
    /// <summary>
    /// A helper when you need the UserHelper class but you dont have a LoggedUser.
    /// </summary>
    /// <remarks><![CDATA[
    /// This Helper is for use in background jobs only.
    /// ]]>
    /// </remarks>
    public class UserHelperNull : IUserHelper
    {
        /// <summary>
        /// Fake information to simulate a logged user
        /// </summary>
        public UserIdentity LoggedUser { get { return new UserIdentity(0, "Background"); } }
    }
}