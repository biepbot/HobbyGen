namespace HobbyGenCoreTest
{
    using System;
    using System.Linq;
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
        private GeneralContext context;

        [TestInitialize]
        public void OnInitialize()
        {
            this.user = new User("Bob Bobbington");
            this.user.Hobbies.Add(new Hobby("Bobbing"));
            this.user.Hobbies.Add(new Hobby("Bobbo"));
            this.user.Hobbies.Add(new Hobby("Archery"));

            var options = new DbContextOptionsBuilder<GeneralContext>()
                .UseInMemoryDatabase("HobbyTest")
                .EnableSensitiveDataLogging()
                .Options;

            // Create mem database
            this.context = new GeneralContext(options);
            this.uManager = new UserManager(this.context);
        }

        [TestCleanup]
        public void OnCleanup()
        {
            this.context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod]
        public void CreateUserTest()
        {
            // Create the user
            context.UserItems.Add(this.user);

            // Save to db
            context.SaveChanges();

            // Let's switch to a different data context
            this.OnInitialize();

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
            Assert.AreEqual(this.user.Hobbies, dbUser.Hobbies,
                "Hobbies are not the same");
        }

        [TestMethod]
        public void GetUserTest()
        {
            this.CreateUserTest();

            // Let's switch to a different data context
            this.OnInitialize();

            var dbUser = this.uManager.GetByName(this.user.Name).FirstOrDefault();

            // Check if names match
            Assert.AreEqual(this.user.Name, dbUser.Name,
                "Saved user has different name");

            // Check if hobbies count match
            Assert.IsTrue(this.user.Hobbies.Count == dbUser.Hobbies.Count,
                "Database user has a different amount of hobbies");

            // Check if hobbies match
            Assert.AreEqual(this.user.Hobbies, dbUser.Hobbies,
                "Hobbies are not the same");
        }
    }
}
