namespace HobbyGen.Models
{
    using HobbyGen.Controllers.Extensions;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

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
        /// Gets the hobbies that a user has. Not mapped to hobbies due to data store errors 
        /// (not enough time to fix)
        /// TODO: find out issue why hobbies are not FETCHED from db
        /// </summary>
        [NotMapped]
        public virtual ICollection<string> Hobbies { get; set; }

        /// <summary>
        /// Hobbies as a string
        /// </summary>
        [Required]
        [JsonIgnore]
        public string HobbyString
        {
            get { return string.Join(",", this.Hobbies); }
            set { this.Hobbies = value.Split(',').ToList(); }
        }

        /// <summary>
        /// Gets the name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        protected User()
        {
            this.Hobbies = new HashSet<string>();
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
                   this.Hobbies.ScrambledEquals(user.Hobbies) &&
                   Name == user.Name;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 444648493;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ICollection<string>>.Default.GetHashCode(Hobbies);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
