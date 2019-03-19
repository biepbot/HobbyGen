namespace HobbyGenCoreTest
{
    using System.Linq;
    using HobbyGen.Controllers.Extensions;
    using HobbyGen.Controllers.Managers;
    using HobbyGen.Models;
    using HobbyGenCoreTest.Base;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for testing user persistence
    /// </summary>
    [TestClass]
    public class UserTest : DatabaseTest
    {
        private User user;
        private UserManager uManager;
        private HobbyManager hManager;

        [TestInitialize]
        public override void OnInitialize()
        {
            // Create a user
            this.user = new User("Bob Bobbington");
            this.user.Hobbies.Add("Bobbing");
            this.user.Hobbies.Add("Bobbo");
            this.user.Hobbies.Add("Archery");

            base.OnInitialize();
        }

        /// <summary>
        /// Tests whether users are equal to each other by using Equals()
        /// </summary>
        [TestMethod]
        public void UserEqualsTest()
        {
            var u1 = new User("Bob Bobbington");
            u1.Hobbies.Add("Bobbing");
            u1.Hobbies.Add("Bobbo");
            u1.Hobbies.Add("Archery");

            var u2 = new User("Bob Bobbington");
            u2.Hobbies.Add("Bobbing");
            u2.Hobbies.Add("Bobbo");
            u2.Hobbies.Add("Archery");

            Assert.AreEqual(u1, u2, "Equals check did not pass");
        }

        /// <summary>
        /// Test if a user can be successfully created
        /// </summary>
        [TestMethod]
        public void CreateUserTest()
        {
            // Create the user
            uManager.CreateUser(this.user);

            // Check if these hobbies are also present in the database
            foreach (var hobby in this.user.Hobbies)
            {
                Assert.IsNotNull(this.hManager.GetByName(hobby));
            }

            // Let's switch to a different data context
            this.ResetContext();

            // Check if added
            Assert.IsTrue(context.UserItems.Count() == 1,
                "Database does not contain users");

            var dbUser = context.UserItems.First();

            // Check if names match
            Assert.AreEqual(this.user.Name, dbUser.Name,
                "Saved user has different name");

            // Check if hobbies count match
            Assert.IsTrue(this.user.Hobbies.Count == dbUser.Hobbies.Count,
                "Database user has a different amount of hobbies");

            // Check if ANY hobbies ok?
            Assert.IsTrue(this.user.Hobbies.Count > 0,
                "User has no hobbies");

            // Check if hobbies match
            Assert.IsTrue(this.user.Hobbies.ScrambledEquals(dbUser.Hobbies),
                "Hobbies are not the same");

            // Check if users can be matched through a Equals check
            Assert.AreEqual(this.user, dbUser, "Users are different");
        }

        /// <summary>
        /// Check if a user can be retrieved from the database successfully
        /// </summary>
        [TestMethod]
        public void GetUserTest()
        {
            this.CreateUserTest();

            // Let's switch to a different data context
            this.ResetContext();

            var dbUser = this.uManager.SearchByName(this.user.Name).FirstOrDefault();

            // Check if users match
            Assert.AreEqual(this.user, dbUser, "Users are different");
        }

        /// <summary>
        /// Check if a user can be updated through the database
        /// </summary>
        [TestMethod]
        public void UpdateUserTest()
        {
            this.CreateUserTest();

            // Let's switch to a different data context
            this.ResetContext();

            var dbUser = this.uManager.SearchByName(this.user.Name).FirstOrDefault();

            // Check if users match
            Assert.AreEqual(this.user, dbUser, "Users are different");

            // Update user with a new hobby
            // Bob now likes swimming!
            this.uManager.UpdateUser(dbUser.Id, new string[] { "Swimming" }, new string[] { });
            this.ResetContext();

            var dbUser2 = this.uManager.GetById(dbUser.Id);

            // Bob should now like swimming
            Assert.IsTrue(dbUser2.Hobbies.Contains("Swimming"));

            // Check if swimming is also present in the database
            Assert.IsNotNull(this.hManager.GetByName("swimming"));

            // Bob decides to hate swimming :(
            this.uManager.UpdateUser(dbUser.Id, new string[] { }, new string[] { "Swimming" });
            this.ResetContext();

            var dbUser3 = this.uManager.GetById(dbUser.Id);

            // Bob should now no longer like swimming
            Assert.IsFalse(dbUser3.Hobbies.Contains("Swimming"));

            // Bob now likes swimming again - it was in error all along! He hates Archery now though.
            this.uManager.UpdateUser(dbUser.Id, new string[] { "Swimming" }, new string[] { "Archery" });
            this.ResetContext();

            var dbUser4 = this.uManager.GetById(dbUser.Id);

            // Check if said hobbies are gone
            Assert.IsTrue(dbUser4.Hobbies.Contains("Swimming"));
            Assert.IsFalse(dbUser4.Hobbies.Contains("Archery"));
        }

        /// <summary>
        /// Check if a user can be deleted
        /// </summary>
        [TestMethod]
        public void DeleteUserTest()
        {
            this.CreateUserTest();

            // Let's switch to a different data context
            this.ResetContext();

            var dbUser = this.uManager.SearchByName(this.user.Name).FirstOrDefault();

            // Check if users match
            Assert.AreEqual(this.user, dbUser, "Users are different");

            // Delete the user
            this.uManager.DeleteUser(dbUser.Id);

            this.ResetContext();

            // Get the user
            dbUser = this.uManager.GetById(dbUser.Id);

            // User should be null (no longer existant)
            Assert.IsNull(dbUser, "User still exists");
        }

        /// <summary>
        /// Check if the name search works as intended
        /// </summary>
        [TestMethod]
        public void SearchUserByNameTest()
        {
            //
            // Search should:
            //  Check contains in both ways
            //

            // Create a bunch of users
            this.uManager.CreateUser("Hendrik", new string[] { "None" });
            this.uManager.CreateUser("Rik", new string[] { "None" });
            this.uManager.CreateUser("Hendrikus", new string[] { "None" });
            this.uManager.CreateUser("Bob", new string[] { "None" });
            this.uManager.CreateUser("Jansen", new string[] { "None" });
            this.uManager.CreateUser("Sen Jan", new string[] { "None" });

            // Let's switch to a different data context
            this.ResetContext();

            //////////
            // CASE 1
            // Rik - Should match Rik, Hendrik, Hendrikus
            //////////
            var rik = this.uManager.SearchByName("Rik");
            Assert.AreEqual(3, rik.Count(), "Searching for Rik resulted in a unexpected amount");

            //////////
            // CASE 1
            // Jan - Should match Sen Jan and Jansen
            //////////
            var jAN = this.uManager.SearchByName("jAN");
            Assert.AreEqual(2, jAN.Count(), "Searching for jAN resulted in a unexpected amount");

            //////////
            // CASE 1
            // Gibberish - Should match none
            //////////
            var asdfgh = this.uManager.SearchByName("asdfgh");
            Assert.AreEqual(0, asdfgh.Count(), "Searching for asdfgh resulted in a unexpected amount");

            //////////
            // CASE 4
            // Hendrikus - Should match Rik, Hendrik, Hendrikus
            //////////
            var Hendrikus = this.uManager.SearchByName("Hendrikus");
            Assert.AreEqual(3, Hendrikus.Count(), "Searching for Hendrikus resulted in a unexpected amount");
        }

        /// <summary>
        /// Check if the hobby search works as intended
        /// </summary>
        [TestMethod]
        public void SearchUserByHobbyTest()
        {
            //
            // Search should:
            //  Check contains in both ways
            //  Ordered by matched amount
            //

            // Create a bunch of users
            this.uManager.CreateUser("Hendrik", new string[] { "Archery" });
            this.uManager.CreateUser("Rik", new string[] { "Archery" });
            this.uManager.CreateUser("Hendrikus", new string[] { "One", "Archery" });
            this.uManager.CreateUser("Bob", new string[] { "One" });
            this.uManager.CreateUser("Jansen", new string[] { "One" });
            this.uManager.CreateUser("Sen Jan", new string[] { "One" });

            // Let's switch to a different data context
            this.ResetContext();

            //////////
            // CASE 1
            // Archery - 3 results
            //////////
            var one = this.uManager.SearchByHobby(new string[] { "Archery" });
            Assert.AreEqual(3, one.Count(), "Searching for Archery resulted in a unexpected amount");

            //////////
            // CASE 2
            // Fake - 0 results
            //////////
            one = this.uManager.SearchByHobby(new string[] { "Fake" });
            Assert.AreEqual(0, one.Count(), "Searching for Fake resulted in a unexpected amount");

            //////////
            // CASE 3
            // Fake AND Archery - 3 results
            //////////
            one = this.uManager.SearchByHobby(new string[] { "Fake", "Archery" });
            Assert.AreEqual(3, one.Count(), "Searching for Fake AND Archery resulted in a unexpected amount");

            //////////
            // CASE 4
            // One AND Archery - 6 results
            //////////
            one = this.uManager.SearchByHobby(new string[] { "One", "Archery" });
            Assert.AreEqual(6, one.Count(), "Searching for One AND Archery resulted in a unexpected amount");

            //////////
            // CASE 4
            // Fake AND One AND Archery - 1 result
            //////////
            one = this.uManager.SearchByHobby(new string[] { "Fake", "One", "Archery" });
            Assert.AreEqual(1, one.Count(), "Searching for Fake AND One AND Archery resulted in a unexpected amount");
        }

        /// <summary>
        /// Resets the database context
        /// </summary>
        protected override void ResetContext()
        {
            base.ResetContext();
            this.uManager = new UserManager(this.context);
            this.hManager = new HobbyManager(this.context);
        }
    }
}
