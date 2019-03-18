namespace HobbyGen.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Class to store user data
    /// </summary>
    public class User : PersistanceObject
    {
        /// <summary>
        /// Gets the hobbies that a user has
        /// </summary>
        HashSet<Hobby> Hobbies { get; } = new HashSet<Hobby>();

        /// <summary>
        /// Gets the name of the user
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        /// <param name="name">The name of the user</param>
        public User(string name)
        {
            this.Name = name;
        }
    }
}
