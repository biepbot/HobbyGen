namespace HobbyGen.Controllers
{
    using System.Collections.Generic;
    using HobbyGen.Controllers.Managers;
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// WebAPI for communicating with user services
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : DatabaseController
    {
        private UserManager uManager;

        public UserController(GeneralContext context) : base(context)
        {
            this.uManager = new UserManager(context);
        }

        // GET api/user
        [HttpGet]
        public IEnumerable<User> Get() 
            => this.uManager.GetAll();

        // GET api/user/5
        [HttpGet("id/{id}")]
        public User GetById(uint id) 
            => this.uManager.GetById(id);

        // GET api/user/hendrik
        [HttpGet("name/{name}")]
        public IEnumerable<User> GetByName(string name) 
            => this.uManager.GetByName(name);

        // POST api/user
        [HttpPost]
        public User Post([FromForm]string name, [FromForm]IEnumerable<string> hobbies)
            => this.uManager.CreateUser(name, hobbies);

        // PUT api/user/5
        [HttpPut("{id}")]
        public void Put(uint id, [FromBody]IEnumerable<string> hobbiesAdded, [FromBody]IEnumerable<string> hobbiesRemoved)
            => this.uManager.UpdateUser(id, hobbiesAdded, hobbiesRemoved);

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public void Delete(uint id) 
            => this.uManager.DeleteUser(id);
    }
}
