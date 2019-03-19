namespace HobbyGen.Controllers.Managers
{
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages specific user calls
    /// </summary>
    public class UserManager
    {
        private GeneralContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class
        /// </summary>
        /// <param name="context"></param>
        public UserManager(GeneralContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a list of users based on the name
        /// </summary>
        /// <param name="name">The name of the user to find</param>
        /// <returns>A list of users</returns>
        public IEnumerable<User> GetByName(string name)
        {
            var all = this.context.UserItems;

            // Get results where name is similar (contains or contains)
            var users = all.Where(

                u => u.Name.ToLower().Contains(name.ToLower())
                || name.ToLower().Contains(u.Name.ToLower()));

            // Order list (alt. Comparer to name)
            var ulist = users.OrderBy(u => u.Name);

            return ulist;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>A list with all the users</returns>
        public IEnumerable<User> GetAll()
        {
            return this.context.UserItems;
        }

        /// <summary>
        /// Gets a user by its id
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>A user or null</returns>
        public User GetById(uint id)
        {
            var u2 = this.context.UserItems
                .FirstOrDefault(u => u.Id == id);
            return u2;
        }

        /// <summary>
        /// Creates a user with the data given
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="hobbies">The hobbies of the user</param>
        /// <returns>The user created</returns>
        public User CreateUser(string name, IEnumerable<string> hobbies)
        {
            // Create the user
            User user = new User(name);

            // Duplicates not allowed
            hobbies = hobbies.Distinct();

            foreach (string hobby in hobbies)
            {
                // Get hobby
                var h = this.FindOrCreateHobby(hobby);
                user.Hobbies.Add(hobby);
            }

            this.context.UserItems.Add(user);
            this.context.SaveChanges();

            return user;
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="id">The id of the user to update</param>
        /// <param name="hobbiesAdded">The hobbies added</param>
        /// <param name="hobbiesRemoved">The hobbies removed</param>
        public void UpdateUser(uint id, IEnumerable<string> hobbiesAdded, IEnumerable<string> hobbiesRemoved)
        {
            var user = this.GetById(id);

            // Add hobbies
            // Duplicates not allowed
            hobbiesAdded = hobbiesAdded.Distinct();
            foreach (string hobby in hobbiesAdded)
            {
                user.Hobbies.Add(this.FindOrCreateHobby(hobby));
            }

            // Remove hobbies
            foreach (string hobby in hobbiesRemoved)
            {
                user.Hobbies.Remove(this.FindOrCreateHobby(hobby));
            }

            // TODO: use batches
            //var batches = CardRepository.splitList(xx, 100);
            //batches.ForEach(x =>
            //{
            //    context.xx.AddRange(x);
            //    context.SaveChanges();
            //});


            this.context.SaveChanges();
        }

        /// <summary>
        /// Deletes a user by its id
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        public void DeleteUser(uint id)
        {
            var user = this.GetById(id);
            this.context.UserItems.Remove(user);
        }

        /// <summary>
        /// Finds or creates a hobby if none are found.
        /// <para />
        /// </summary>
        /// <param name="name">The name of the hobby to find or create</param>
        /// <returns>A hobby</returns>
        private string FindOrCreateHobby(string name)
        {
            var hobby = this.context.HobbyItems.FirstOrDefault(h => h.Name == name.ToLower());
            if (hobby == null)
            {
                hobby = new Hobby(name);
                this.context.HobbyItems.Add(hobby);
                this.context.SaveChanges();
            }

            return name;
        }
    }
}
