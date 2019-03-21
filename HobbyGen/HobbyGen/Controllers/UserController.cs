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
            => this.uManager.SearchByName(name);

        // GET api/user
        [HttpPost("hobby")]
        public IEnumerable<User> GetByHobby([FromBody]string[] hobbies)
            => this.uManager.SearchByHobby(hobbies);

        // POST api/user
        [HttpPost]
        public User Post([FromBody]User u)
            => this.uManager.CreateUser(u);

        // PUT api/user/5
        [HttpPut("{id}")]
        public void Put(uint id, [FromForm]IEnumerable<string> hobbiesAdded, [FromForm]IEnumerable<string> hobbiesRemoved)
            => this.uManager.UpdateUser(id, hobbiesAdded, hobbiesRemoved);

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public void Delete(uint id)
            => this.uManager.DeleteUser(id);
    }
}
