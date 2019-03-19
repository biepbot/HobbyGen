namespace HobbyGenCoreTest
{
    using System;
    using System.Linq;
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
        private DbContextOptions<GeneralContext> options;

        [TestInitialize]
        public void OnInitialize()
        {
            this.user = new User("Bob Bobbington");
            this.user.Hobbies.Add(new Hobby("Bobbing"));
            this.user.Hobbies.Add(new Hobby("Bobbo"));
            this.user.Hobbies.Add(new Hobby("Archery"));

            this.options = new DbContextOptionsBuilder<GeneralContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
        }

        [TestMethod]
        public void CreateUserTest()
        {
            using (var context = new GeneralContext(this.options))
            {
                context.Database.EnsureDeleted();

                // Create the user
                context.UserItems.Add(this.user);

                // Save to db
                context.SaveChanges();


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

                // Check if hobbies match
                Assert.AreEqual(this.user.Hobbies, dbUser.Hobbies,
                    "Hobbies are not the same");
            };
        }
    }
}
