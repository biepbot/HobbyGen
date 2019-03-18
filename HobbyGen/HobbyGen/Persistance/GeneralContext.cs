namespace HobbyGen.Persistance
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using HobbyGen.Models;
    using HobbyGen.Persistance.Interfaces;

    /// <summary>
    /// Class to manage a general database context
    /// </summary>
    public class GeneralContext : DbContext, IHobbyContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        [Obsolete("Automated constructor method, please refer from making instances of this class")]
        public GeneralContext(DbContextOptions<GeneralContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the user items attached to the database context
        /// </summary>
        public DbSet<User> UserItems { get; set; }

        /// <summary>
        /// Gets or sets the hobby items attached to the database context
        /// </summary>
        public DbSet<Hobby> HobbyItems { get; set; }

        /// <inheritdoc/>
        public int Save()
        {
            return this.SaveChanges();
        }
    }
}
