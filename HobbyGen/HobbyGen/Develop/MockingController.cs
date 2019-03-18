namespace HobbyGen.Controllers
{
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    /// <summary>
    /// Mocking controller for resource injection for tests
    /// </summary>
    [Route("api/ha82y6bxz")]
    public class MockingController : DatabaseController
    {
        public MockingController(GeneralContext context) : base(context)
        {
            if (!this.DataContext.HobbyItems.Any())
            {
                this.CreateUser("Barry Boomsma", new string[] { "Wandelen", "Schaatsen" });
                this.CreateUser("Hendrik Jansen", new string[] { "Koken", "Wandelen" });
                this.DataContext.Save();
            }
        }

        /// <summary>
        /// Creates user with a few variables
        /// </summary>
        /// <param name="user">The user name</param>
        /// <param name="hobbies">The hobbies of these users</param>
        private void CreateUser(string user, string[] hobbies)
        {
            var dbhobbies = this.DataContext.HobbyItems;

            var u = new User(user);
            foreach (string h in hobbies)
            {

                // MOVE HOBBY INSERTION LOGIC ELSEWHERE
                // Find hobby in database
                Hobby dbh = dbhobbies.Find(h);

                // If no hobby was found
                if (dbh == null)
                {
                    // Create the hobby
                    dbh = new Hobby(h);

                    // insert hobby into database?
                    dbhobbies.Add(dbh);
                }

                // add hobby to user
                u.Hobbies.Add(dbh);
            }

            // Add user to database
            this.DataContext.UserItems.Add(u);
            this.DataContext.Save();
        }
    }
}
