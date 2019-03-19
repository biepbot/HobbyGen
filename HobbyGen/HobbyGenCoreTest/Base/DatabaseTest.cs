namespace HobbyGenCoreTest.Base
{
    using HobbyGen.Persistance;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Base testing class for any interaction with datbase contexts
    /// </summary>
    [TestClass]
    public class DatabaseTest
    {
        protected GeneralContext context;

        [TestInitialize]
        public virtual void OnInitialize()
        {
            this.ResetContext();
        }

        [TestCleanup]
        public virtual void OnCleanup()
        {
            // Clear the database
            this.context.Database.EnsureDeleted();

            // Throw away the database context
            this.context.Dispose();
        }

        /// <summary>
        /// Resets the database context and creates a new context to work with
        /// </summary>
        protected virtual void ResetContext()
        {
            var options = new DbContextOptionsBuilder<GeneralContext>()
                .UseInMemoryDatabase("HobbyTest")
                .EnableSensitiveDataLogging()
                .Options;

#pragma warning disable CS0618 // Type or member is obsolete
            // Create mem database
            this.context = new GeneralContext(options);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
