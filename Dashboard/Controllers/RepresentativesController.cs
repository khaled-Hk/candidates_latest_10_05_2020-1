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
    [Route("Api/Admin/Representatives")]
    public class RepresentativesController : Controller
    {
        private readonly CandidatesContext db;
        private Helper help;
        public RepresentativesController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        //[HttpGet("Get")]
        //public IActionResult Get(int pageNo, int pageSize)
        //{
        //    try
        //    {
        //        var EntitesCount = db.Entities.Where(x => x.Status != 9).Count();
        //        var EntitesList = db.Entities.Where(x => x.Status != 9)
        //            .OrderByDescending(x => x.CreatedOn)
        //            .Select(x => new
        //            {
        //               x.Address,
        //               x.CreatedBy,
        //               x.CreatedOn,
        //               x.Descriptions,
        //               x.Email,
        //               x.EntityId,
        //               //x.EntityRepresentatives,
        //               //x.EntityUsers,
        //               x.Name,
        //               x.Number,
        //               x.Owner,
        //               x.Phone,
        //               x.Status
        //            }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        //        return Ok(new { Entites = EntitesList, count = EntitesCount });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


        [HttpPost("Add")]
        public IActionResult AddRepresentatives([FromBody] Entities Entity)
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

                if (string.IsNullOrWhiteSpace(Entity.Email))
                {
                    return StatusCode(404, "الرجاء إدخال البريد الالكتروني");
                }
                if (!Common.Validation.IsValidEmail(Entity.Email))
                {
                    return StatusCode(404, "الرجاء التأكد من البريد الإلكتروني");
                }
                if (string.IsNullOrWhiteSpace(Entity.Phone))
                {
                    return StatusCode(404, "الرجاء إدخال رقم الهاتف");
                }
                if (string.IsNullOrWhiteSpace(Entity.Phone))
                {
                    return StatusCode(404, "الرجاء إدخال رقم الهاتف");
                }
                if (Entity.Phone.Length < 9)
                {
                    return StatusCode(404, "الرجاء إدخال الهـاتف بطريقة الصحيحة !!");
                }
                Entity.Phone = Entity.Phone.Substring(Entity.Phone.Length - 9);

                if (Entity.Phone.Substring(0, 2) != "91" && Entity.Phone.Substring(0, 2) != "92" && Entity.Phone.Substring(0, 2) != "94")
                {
                    return StatusCode(404, "يجب ان يكون الهاتف يبدأ ب (91,92,94) ليبيانا او المدار !!");
                }




                var NameExist = db.Entities.Where(x => x.Name == Entity.Name && x.Status != 9).SingleOrDefault();
                if(NameExist!=null)
                    return BadRequest("اسم الكيان موجود مسبقا الرجاء إعادة الادخال");

                var NumberExist = db.Entities.Where(x => x.Number == Entity.Number && x.Status != 9).SingleOrDefault();
                if (NumberExist != null)
                    return BadRequest("رقم الكيان موجود مسبقا الرجاء إعادة الادخال");

                var PhoneExist = db.Entities.Where(x => x.Phone == Entity.Phone && x.Status != 9).SingleOrDefault();
                if (PhoneExist != null)
                    return BadRequest("رقم الهاتف موجود مسبقا الرجاء إعادة الادخال");

                var EmailExist = db.Entities.Where(x => x.Email == Entity.Email && x.Status != 9).SingleOrDefault();
                if (EmailExist != null)
                    return BadRequest("البريد الإلكتروني موجود مسبقا الرجاء إعادة الادخال");


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