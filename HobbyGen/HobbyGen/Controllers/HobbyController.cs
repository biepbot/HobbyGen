namespace HobbyGen.Controllers
{
    using System.Collections.Generic;
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class HobbyController : DatabaseController
    {
        public HobbyController(GeneralContext context) : base(context) { }

        // GET api/hobby
        [HttpGet]
        public IEnumerable<Hobby> Get()
        {
            return this.DataContext.HobbyItems;
        }

        // GET api/hobby/Schaatsen
        [HttpGet("{id}")]
        public Hobby Get(string id)
        {
            return this.DataContext.HobbyItems.Find(id.ToLower());
        }

        // POST api/hobby?name=Schaatsen
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Post([FromQuery]string name)
        {
            var match = this.Get(name);
            if (match == null) // Hobby doesn't exist!
            {
                // add new hobby
                this.DataContext.HobbyItems.Add(new Hobby(name));
                this.DataContext.Save();
            }
            else // Hobby exists!
            {
                // 409 - Conflict
                return this.StatusCode(409);
            }
            return this.Ok();
        }

        // DELETE api/hobby/Schaatsen
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            // TODO: Only allow if there's no users bound to this one, else admin only
            // currently always allows

            var match = this.Get(id);
            if (match != null) // Hobby exists!
            {
                // Delete the hobby
                this.DataContext.HobbyItems.Remove(match);
                this.DataContext.Save();
            }
        }
    }
}
