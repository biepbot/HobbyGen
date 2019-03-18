namespace HobbyGen.Persistance.Interfaces
{
    using HobbyGen.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Hobby context interface
    /// </summary>
    public interface IHobbyContext
    {
        /// <summary>
        /// Gets or sets the user items attached to the database context
        /// </summary>
        DbSet<User> UserItems { get; set; }

        /// <summary>
        /// Gets or sets the hobby items attached to the database context
        /// </summary>
        DbSet<Hobby> HobbyItems { get; set; }
    }
}
