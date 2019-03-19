namespace HobbyGen.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class to store user data
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the Id of this user
        /// </summary>
        [Key]
        public uint Id { get; set; }

        /// <summary>
        /// Gets the hobbies that a user has
        /// </summary>
        // use virtual for lazy loading
        public virtual ICollection<Hobby> Hobbies { get; set; }

        /// <summary>
        /// Gets the name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        protected User()
        {
            this.Hobbies = new HashSet<Hobby>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        /// <param name="name">The name of the user</param>
        public User(string name) : this()
        {
            this.Name = name;
        }
    }
}
