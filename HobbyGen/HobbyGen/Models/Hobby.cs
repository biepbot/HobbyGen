namespace HobbyGen.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class that specifies data about hobbies
    /// </summary>
    public class Hobby
    {
        [Key]
        [JsonIgnore]
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the name of a hobby
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hobby"/> class
        /// </summary>
        protected Hobby() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hobby"/> class
        /// </summary>
        /// <param name="name">The name of the hobby</param>
        public Hobby(string name)
        {
            this.Name = name.ToLower();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var hobby = obj as Hobby;
            return hobby != null &&
                   Name == hobby.Name;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
