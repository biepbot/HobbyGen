namespace HobbyGen.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// WebAPI for communicating with user services
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : DatabaseController
    {
        public UserController(GeneralContext context) : base(context) { }

        // GET api/user
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return this.DataContext.UserItems;
        }

        // GET api/user/5
        [HttpGet("id/{id}")]
        public User GetById(uint id)
        {
            return this.DataContext.UserItems
                .FirstOrDefault(u => u.Id == id);
        }

        // GET api/user/hendrik
        [HttpGet("name/{name}")]
        public IEnumerable<User> GetByName(string name)
        {
            var all = this.DataContext.UserItems;

            // Get results where name is similar (contains or contains)
            var users = all.Where(

                u => u.Name.ToLower().Contains(name.ToLower())
                || name.ToLower().Contains(u.Name.ToLower()));

            // Order list (alt. Comparer to name)
            var ulist = users.OrderBy(u => u.Name);

            return ulist;
        }

        // POST api/user
        [HttpPost]
        public User Post([FromForm]string name, [FromForm]IEnumerable<string> hobbies)
        {
            // Create the user
            User user = new User(name);

            // Duplicates not allowed
            hobbies = hobbies.Distinct();

            foreach (string hobby in hobbies)
            {
                // Get hobby
                var h = this.FindOrCreateHobby(hobby);
                user.Hobbies.Add(h);
            }

            this.DataContext.UserItems.Add(user);
            this.DataContext.SaveChanges();

            return user;
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public void Put(uint id, [FromBody]IEnumerable<string> hobbiesAdded, [FromBody]IEnumerable<string> hobbiesRemoved)
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


            this.DataContext.SaveChanges();
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public void Delete(uint id)
        {
            var user = this.GetById(id);
            this.DataContext.UserItems.Remove(user);
        }

        /// <summary>
        /// Finds or creates a hobby if none are found.
        /// <para />
        /// TODO: move to manager
        /// </summary>
        /// <param name="name">The name of the hobby to find or create</param>
        /// <returns>A hobby</returns>
        private Hobby FindOrCreateHobby(string name)
        {
            var hobby = this.DataContext.HobbyItems.FirstOrDefault(h => h.Name == name.ToLower());
            if (hobby == null)
            {
                hobby = new Hobby(name);
            }

            return hobby;
        }
    }
}
