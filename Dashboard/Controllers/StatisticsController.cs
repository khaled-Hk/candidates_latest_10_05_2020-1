using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using static Services.Helper;

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
                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }
                var candidatesCount = db.Candidates.Where(x => x.Status == 1 && x.ProfileId == UP.ProfileId).Count();
                var independentCandidatesCount = db.Candidates.Where(x => x.Status == 1 && x.ProfileId == UP.ProfileId && x.EntityId == null).Count();
                var candidatesCountByEntities = db.Candidates.Where(x => x.ProfileId == UP.ProfileId && x.Status == 1 && x.EntityId != null).Count();
                var entitiesCount = db.Entities.Where(x => x.Status == 1 && x.ProfileId == UP.ProfileId).Count();
                return Ok(new { candidatesCount, independentCandidatesCount, candidatesCountByEntities, entitiesCount });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
