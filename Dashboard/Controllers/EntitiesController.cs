using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Vue.Controllers
{
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    [Route("Api/Admin/Entities")]
    public class EntitiesController : Controller
    {
        private readonly CandidatesContext db;
        private Helper help;
        public EntitiesController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {
                var EntitesCount = db.Entities.Where(x => x.Status != 9).Count();
                var EntitesList = db.Entities.Where(x => x.Status != 9)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new
                    {
                       x.Address,
                       x.CreatedBy,
                       x.CreatedOn,
                       x.Descriptions,
                       x.Email,
                       x.EntityId,
                       x.EntityRepresentatives,
                       x.EntityUsers,
                       x.Name,
                       x.Number,
                       x.Owner,
                       x.Phone,
                       x.Status
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { Entites = EntitesList, count = EntitesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("Add")]
        public IActionResult AddEntity([FromBody] Entities Entity)
        {
            try
            {
                if (Entity == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }


                Entity.CreatedBy = userId;
                Entity.CreatedOn = DateTime.Now;
                db.Entities.Add(Entity);
                db.SaveChanges();

                return Ok(" تم اضافة الكيان السياسي  بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }










    }
}