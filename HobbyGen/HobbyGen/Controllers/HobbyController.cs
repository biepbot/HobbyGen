namespace HobbyGen.Controllers
{
    using System.Collections.Generic;
    using HobbyGen.Controllers.Managers;
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// WebAPI for communicating with hobby services
    /// </summary>
    [Route("api/[controller]")]
    public class HobbyController : DatabaseController
    {
        private HobbyManager hManager;

        public HobbyController(GeneralContext context) : base(context)
        {
            this.hManager = new HobbyManager(context);
        }

        // GET api/hobby
        [HttpGet]
        public IEnumerable<Hobby> Get() 
            => this.hManager.GetAll();

        // GET api/hobby/Schaatsen
        [HttpGet("{id}")]
        public Hobby Get(string id) 
            => this.hManager.GetByName(id);

        // POST api/hobby?name=Schaatsen
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Post([FromQuery]string name) 
            => StatusCode(this.hManager.CreateHobby(name));

        // DELETE api/hobby/Schaatsen
        [HttpDelete("{id}")]
        public void Delete(string id) 
            => this.hManager.DeleteHobby(id);
    }
}
