namespace HobbyGenCoreTest
{
    using System.Linq;
    using HobbyGen.Controllers.Managers;
    using HobbyGenCoreTest.Base;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for testing hobby persistence
    /// </summary>
    [TestClass]
    public class HobbyTest : DatabaseTest
    {
        private HobbyManager hManager;
        private UserManager uManager;

        /// <summary>
        /// Resets the database context
        /// </summary>
        protected override void ResetContext()
        {
            base.ResetContext();
            this.hManager = new HobbyManager(this.context);
            this.uManager = new UserManager(this.context);
        }

        /// <summary>
        /// Check if a hobby can be made
        /// </summary>
        [TestMethod]
        public void HobbyCreationTest()
        {
            // Create hobby
            this.hManager.CreateHobby("Abc");

            // Let's switch to a different data context
            this.ResetContext();

            // Get hobbies
            var hobby = this.hManager.GetByName("Abc");

            Assert.IsNotNull(hobby, "No hobby with name Abc found");
        }

        /// <summary>
        /// Check if a hobby can be deleted
        /// </summary>
        [TestMethod]
        public void HobbyDeletionTest()
        {
            // Create hobby
            this.HobbyCreationTest();

            var hobby = this.hManager.GetAll().FirstOrDefault();
            Assert.IsNotNull(hobby, "No hobby found to delete");

            // Delete hobby
            this.hManager.DeleteHobby(hobby.Name);
            var deletedHobby = this.hManager.GetByName(hobby.Name);
            Assert.IsNull(deletedHobby, "Hobby was not deleted");
        }


        /// <summary>
        /// Check if a hobby can be deleted from users indirectly
        /// </summary>
        [TestMethod]
        public void BigHobbyDeletionTest()
        {
            // Create users with hobby
            var hobby = "Minecraft";
            var deletHobby = "Fortnite";

            this.uManager.CreateUser("Bob Bobbington", new string[] { hobby, deletHobby });
            this.uManager.CreateUser("Steve", new string[] { hobby });
            this.uManager.CreateUser("Hendrik", new string[] { deletHobby });
            this.uManager.CreateUser("Bobbius", new string[] { hobby, deletHobby });
            this.uManager.CreateUser("Bobbard", new string[] { hobby, deletHobby });

            // Let's switch to a different data context
            this.ResetContext();

            // Assert the numbers are correct at start
            var hCount = this.uManager.SearchByHobby(new string[] { hobby }); // 4
            var dhCount = this.uManager.SearchByHobby(new string[] { deletHobby }); // 4

            // 4 made, none deleted
            Assert.AreEqual(4, hCount.Count());

            // 4 made as well
            Assert.AreEqual(4, hCount.Count());

            // Let's switch to a different data context
            this.ResetContext();

            // Delete the hobby
            this.hManager.DeleteHobby(deletHobby);
            var deletedHobby = this.hManager.GetByName(deletHobby);
            Assert.IsNull(deletedHobby, "Hobby was not deleted");

            // Assert that there's less users with hobby "deletHobby" and same for hobby "hobby"
            hCount = this.uManager.SearchByHobby(new string[] { hobby }); // 4
            dhCount = this.uManager.SearchByHobby(new string[] { deletHobby }); // 0

            // 4 made, none deleted
            Assert.AreEqual(4, hCount.Count());

            // all deleted, so none found
            Assert.IsFalse(dhCount.Any());
        }
    }
}
