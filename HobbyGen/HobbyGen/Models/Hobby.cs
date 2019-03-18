namespace HobbyGen.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class that specifies data about hobbies
    /// </summary>
    public class Hobby
    {
        /// <summary>
        /// Gets or sets the name of a hobby
        /// </summary>
        [Key]
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
    }
}
