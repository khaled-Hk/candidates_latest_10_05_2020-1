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
        public IActionResult AddRepresentatives([FromBody] CandidateRepresentatives Representatives)
        {
            try
            {
                if (Representatives == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (string.IsNullOrWhiteSpace(Representatives.Email))
                {
                    return StatusCode(404, "الرجاء إدخال البريد الالكتروني");
                }
                if (!Common.Validation.IsValidEmail(Representatives.Email))
                {
                    return StatusCode(404, "الرجاء التأكد من البريد الإلكتروني");
                }
                if (string.IsNullOrWhiteSpace(Representatives.Phone))
                {
                    return StatusCode(404, "الرجاء إدخال رقم الهاتف");
                }
                if (Representatives.Phone.Length < 9)
                {
                    return StatusCode(404, "الرجاء إدخال الهـاتف بطريقة الصحيحة !!");
                }
                Representatives.Phone = Representatives.Phone.Substring(Representatives.Phone.Length - 9);

                if (Representatives.Phone.Substring(0, 2) != "91" && Representatives.Phone.Substring(0, 2) != "92" && Representatives.Phone.Substring(0, 2) != "94")
                {
                    return StatusCode(404, "يجب ان يكون الهاتف يبدأ ب (91,92,94) ليبيانا او المدار !!");
                }




                var NIDExist = db.CandidateRepresentatives.Where(x => x.Nid == Representatives.Nid ).SingleOrDefault();
                if(NIDExist!=null)
                    return BadRequest("الرقم الوطني موجود مسبقا الرجاء إعادة الادخال");

                var PhoneExist = db.CandidateRepresentatives.Where(x => x.Phone == Representatives.Phone ).SingleOrDefault();
                if (PhoneExist != null)
                    return BadRequest("رقم الهاتف موجود مسبقا الرجاء إعادة الادخال");

                var EmailExist = db.Entities.Where(x => x.Email == Representatives.Email && x.Status != 9).SingleOrDefault();
                if (EmailExist != null)
                    return BadRequest("البريد الإلكتروني موجود مسبقا الرجاء إعادة الادخال");


                Representatives.CreatedBy = userId;
                Representatives.CreatedOn = DateTime.Now;
                db.CandidateRepresentatives.Add(Representatives);
                db.SaveChanges();

                return Ok(" تم اضافة الممثل  بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }










    }
}