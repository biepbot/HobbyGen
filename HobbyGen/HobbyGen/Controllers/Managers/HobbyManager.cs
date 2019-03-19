namespace HobbyGen.Controllers.Managers
{
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using System.Collections.Generic;
    using System.Linq;

    public class HobbyManager
    {
        private GeneralContext context;
        private UserManager uManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HobbyManager"/> class
        /// </summary>
        /// <param name="context"></param>
        public HobbyManager(GeneralContext context)
        {
            this.context = context;
            this.uManager = new UserManager(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HobbyManager"/> class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="uManager">The usermanager to use</param>
        public HobbyManager(GeneralContext context, UserManager uManager)
        {
            this.context = context;
            this.uManager = uManager;
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
        /// <param name="name">The hobby</param>
        /// <returns>The hobby</returns>
        public Hobby GetByName(string name)
        {
            name = name.ToLower();
            return this.context.HobbyItems.FirstOrDefault(h => h.Name == name);
        }
        
        /// <summary>
        /// Creates a hobby by its name
        /// </summary>
        /// <param name="name">The name of the hobby</param>
        /// <returns>Status code</returns>
        public int CreateHobby(string name)
        {
            var match = this.GetByName(name);
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
        /// Deletes a hobby by its name
        /// </summary>
        /// <param name="name">The name of the hobby</param>
        public void DeleteHobby(string name)
        {
            // TODO: Only allow if there's no users bound to this one, else admin only
            // currently always allows

            var match = this.GetByName(name);
            if (match != null) // Hobby exists!
            {
                // TODO: Delete hobby from all users with this hobby
                //
                // Get all users with this hobby
                var affectedUsers = this.uManager.SearchByHobby(new string[] { name });
                
                foreach (var user in affectedUsers)
                {
                    // Remove this hobby from the user
                    user.Hobbies.Remove(name);

                    // Update the user
                    this.context.Update(user);
                }


                // Delete the hobby
                this.context.HobbyItems.Remove(match);
                this.context.SaveChanges();
            }
        }

        /// <summary>
        /// Finds or creates a hobby if none are found.
        /// <para />
        /// </summary>
        /// <param name="name">The name of the hobby to find or create</param>
        /// <returns>A hobby</returns>
        public string FindOrCreateHobby(string name)
        {
            var hobby = this.GetByName(name);
            if (hobby == null)
            {
                this.CreateHobby(name);
            }

            return name;
        }
    }
}
