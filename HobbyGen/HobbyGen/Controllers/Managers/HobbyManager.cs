namespace HobbyGen.Controllers.Managers
{
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using System.Collections.Generic;
    using System.Linq;

    public class HobbyManager
    {
        private GeneralContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HobbyManager"/> class
        /// </summary>
        /// <param name="context"></param>
        public HobbyManager(GeneralContext context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Gets all the hobbies
        /// </summary>
        /// <returns>A list of all the hobbies</returns>
        public IEnumerable<Hobby> GetAll()
        {
            return this.context.HobbyItems;
        }
        
        /// <summary>
        /// Gets a hobby by its name
        /// </summary>
        /// <param name="id">The hobby</param>
        /// <returns>The hobby</returns>
        public Hobby GetById(string id)
        {
            return this.context.HobbyItems.Find(id.ToLower());
        }
        
        /// <summary>
        /// Creates a hobby by its name
        /// </summary>
        /// <param name="name">The name of the hobby</param>
        /// <returns>Status code</returns>
        public int CreateHobby(string name)
        {
            var match = this.GetById(name);
            if (match == null) // Hobby doesn't exist!
            {
                // add new hobby
                this.context.HobbyItems.Add(new Hobby(name));
                this.context.SaveChanges();
            }
            else // Hobby exists!
            {
                // 409 - Conflict
                return 409;
            }
            return 200;
        }
        
        /// <summary>
        /// Deletes a hobby by its id
        /// </summary>
        /// <param name="id">The name of the hobby</param>
        public void DeleteHobby(string id)
        {
            // TODO: Only allow if there's no users bound to this one, else admin only
            // currently always allows

            var match = this.GetById(id);
            if (match != null) // Hobby exists!
            {
                // Delete the hobby
                this.context.HobbyItems.Remove(match);
                this.context.SaveChanges();
            }
        }
    }
}
