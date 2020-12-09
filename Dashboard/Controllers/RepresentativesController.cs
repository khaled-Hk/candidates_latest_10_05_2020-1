using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Vue.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class RepresentativesController : ControllerBase
    {
        private readonly CandidatesContext db;
        private Helper help;
        public RepresentativesController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        [HttpGet("GetRepresentativesByEntityId/{id}")]
        public IActionResult GetRepresentativesByEntityId([FromRoute]long? id)
        {
            try
            {
                var CandidateRepresentatives=db.EntityRepresentatives.Where(x=> x.EntityId==id).Select(x=>new {
                    Name = string.Format("{0} {1} {2} {3}", x.FirstName, x.FatherName, x.GrandFatherName, x.SurName),
                    x.Nid,
                    x.MotherName,
                    x.Phone,
                    x.Gender,
                    x.Email,
                    x.CreatedOn,
                    x.BirthDate,
                }).ToList();


                return Ok(new { representatives = CandidateRepresentatives, count = CandidateRepresentatives.Count() });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }


        [HttpPost("Add")]
        public IActionResult AddRepresentatives([FromBody] EntityRepresentatives Representatives)
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

               




                var NIDExist = db.EntityRepresentatives.Where(x => x.Nid == Representatives.Nid ).SingleOrDefault();
                if(NIDExist!=null)
                    return BadRequest("الرقم الوطني موجود مسبقا الرجاء إعادة الادخال");

                var PhoneExist = db.EntityRepresentatives.Where(x => x.Phone == Representatives.Phone ).SingleOrDefault();
                if (PhoneExist != null)
                    return BadRequest("رقم الهاتف موجود مسبقا الرجاء إعادة الادخال");

                var EmailExist = db.EntityRepresentatives.Where(x => x.Email == Representatives.Email ).SingleOrDefault();
                if (EmailExist != null)
                    return BadRequest("البريد الإلكتروني موجود مسبقا الرجاء إعادة الادخال");

                if (string.IsNullOrWhiteSpace(Representatives.BirthDate.ToString()))
                {
                    return BadRequest("الرجاء دخال تاريخ الميلاد ");
                }
                if ((DateTime.Now.Year - Representatives.BirthDate.GetValueOrDefault().Year) < 18)
                {
                    return BadRequest("يجب ان يكون عمر الممثل اكبر من 18");
                }


                Representatives.CreatedBy = userId;
                Representatives.CreatedOn = DateTime.Now;
                db.EntityRepresentatives.Add(Representatives);
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