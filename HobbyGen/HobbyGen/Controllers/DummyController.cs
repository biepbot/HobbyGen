namespace HobbyGen.Controllers
{
    using HobbyGen.Controllers.Managers;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// WebAPI for communicating with user services
    /// </summary>
    [Route("api/[controller]")]
    public class DummyController : DatabaseController
    {
        private UserManager uManager;

        public DummyController(GeneralContext context) : base(context)
        {
            this.uManager = new UserManager(context);
        }

        // POST api/dummy
        [HttpPost]
        public void Get()
            => Mocker.FillDatabase(100, 10, 1000, this.uManager);
    }
}
