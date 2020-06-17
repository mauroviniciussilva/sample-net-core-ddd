namespace Sample.Domain.Entities.Helpers
{
    /// <summary>
    /// Identifier of the current logged user
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// Primary key that identifies the user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        public UserIdentity(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
