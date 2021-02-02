using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vue.Controllers
{
    
    [Produces("application/json")]
    [Route("Api/Admin/[controller]")]
    public class StatisticsController : Controller
    {
        private readonly CandidatesContext db;
        private Helper help;

        public StatisticsController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        [HttpGet]
        public IActionResult GetHomePageStatistics()
        {
            try
            {
                var profile = db.Profile.Where(x => x.Status == 1).SingleOrDefault();
                var candidatesCount = db.Candidates.Where(x => x.Status == 1 && x.ProfileId == profile.ProfileId).Count();
                var independentCandidatesCount = db.Candidates.Where(x => x.Status == 1 && x.ProfileId == profile.ProfileId && x.EntityId == null).Count();
                var candidatesCountByEntities = db.Candidates.Where(x => x.ProfileId == profile.ProfileId && x.Status == 1 && x.EntityId != null).Count();
                var entitiesCount = db.Entities.Where(x => x.Status == 1 && x.ProfileId == profile.ProfileId).Count();
                return Ok(new { candidatesCount, independentCandidatesCount, candidatesCountByEntities, entitiesCount });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
