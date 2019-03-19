namespace HobbyGenCoreTest
{
    using System.Linq;
    using HobbyGen.Controllers.Extensions;
    using HobbyGen.Controllers.Managers;
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for testing user persistence
    /// </summary>
    [TestClass]
    public class UserTest
    {
        private User user;
        private UserManager uManager;
        private HobbyManager hManager;
        private GeneralContext context;

        [TestInitialize]
        public void OnInitialize()
        {
            this.user = new User("Bob Bobbington");
            this.user.Hobbies.Add("Bobbing");
            this.user.Hobbies.Add("Bobbo");
            this.user.Hobbies.Add("Archery");

            this.ResetContext();
        }

        [TestCleanup]
        public void OnCleanup()
        {
            this.context.Database.EnsureDeleted();
            context.Dispose();
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

            Assert.IsTrue(dbUser4.Hobbies.Contains("Swimming"));
            Assert.IsFalse(dbUser4.Hobbies.Contains("Archery"));

        }

        /// <summary>
        /// Resets the database context
        /// </summary>
        private void ResetContext()
        {
            var options = new DbContextOptionsBuilder<GeneralContext>()
                .UseInMemoryDatabase("HobbyTest")
                .EnableSensitiveDataLogging()
                .Options;

#pragma warning disable CS0618 // Type or member is obsolete
            // Create mem database
            this.context = new GeneralContext(options);
            this.uManager = new UserManager(this.context);
            this.hManager = new HobbyManager(this.context);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
