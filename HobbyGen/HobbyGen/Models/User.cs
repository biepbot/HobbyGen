namespace HobbyGen.Models
{
    using HobbyGen.Controllers.Extensions;
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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (user == null)
            {
                return false;
            }

            return user != null &&
                   Id == user.Id &&
                   this.Hobbies.ScrambledEquals(user.Hobbies) &&
                   Name == user.Name;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 444648493;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ICollection<Hobby>>.Default.GetHashCode(Hobbies);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
