namespace HobbyGen.Controllers
{
    using HobbyGen.Persistance;
    using HobbyGen.Persistance.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Database controller class for quick database injection
    /// </summary>
    public class DatabaseController : Controller
    {
        /// <summary>
        /// The database context to use
        /// </summary>
        protected IHobbyContext DataContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseController"/> class
        /// </summary>
        /// <param name="context">The context to use for database queries</param>
        public DatabaseController(GeneralContext context)
        {
            this.DataContext = context;
        }
    }
}
