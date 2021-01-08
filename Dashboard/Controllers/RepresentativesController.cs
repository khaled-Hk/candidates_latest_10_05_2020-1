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


        //[HttpPost("Candidate/Add")]
        //public IActionResult AddCandidateRepresentatives([FromBody] CandidateRepresentatives candidateRepresentatives)
        //{
        //    try
        //    {
        //        if (candidateRepresentatives == null)
        //        {
        //            return BadRequest(new { message = "حذث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
        //        }

        //        var userId = this.help.GetCurrentUser(HttpContext);

        //        if (userId <= 0)
        //        {
        //            return StatusCode(401, new { message = "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" });
        //        }


        //        if (candidateRepresentatives.CandidateId == 0 || candidateRepresentatives.CandidateId == null)
        //        {
        //            return StatusCode(404, new { message = "الرجاء إدخال الرقم الوطني" });
        //        }

        //        if (string.IsNullOrEmpty(candidateRepresentatives.Nid))
        //        {
        //            return StatusCode(404, new { message = "الرجاء إدخال الرقم الوطني" });
        //        }

        //        if (candidateRepresentatives.Nid.Length == 12)
        //        {
        //            return StatusCode(404, new { message = "يجب أن يكون طول الرقم الوطني 12 رقماً" });
        //        }

        //        if (string.IsNullOrWhiteSpace(candidateRepresentatives.Email))
        //        {
        //            return StatusCode(404, new { message = "الرجاء إدخال البريد الالكتروني" });
        //        }
        //        if (!Common.Validation.IsValidEmail(candidateRepresentatives.Email))
        //        {
        //            return StatusCode(404, new { message = "الرجاء التأكد من البريد الإلكتروني" });
        //        }
        //        if (string.IsNullOrWhiteSpace(candidateRepresentatives.Phone))
        //        {
        //            return StatusCode(404, new { message = "الرجاء إدخال رقم الهاتف" });
        //        }
        //        if (candidateRepresentatives.Phone.Length < 9)
        //        {
        //            return StatusCode(404, new { message = "الرجاء إدخال الهـاتف بطريقة الصحيحة !!" });
        //        }
        //        candidateRepresentatives.Phone = candidateRepresentatives.Phone.Substring(candidateRepresentatives.Phone.Length - 9);

        //        if (candidateRepresentatives.Phone.Substring(0, 2) != "91" && candidateRepresentatives.Phone.Substring(0, 2) != "92" && candidateRepresentatives.Phone.Substring(0, 2) != "94" && candidateRepresentatives.Phone.Substring(0, 2) != "93" && candidateRepresentatives.Phone.Substring(0, 2) != "95")
        //        {
        //            return StatusCode(404, new { message = "يجب ان يكون الهاتف يبدأ ب (91,92,93,94,95)   ليبيانا او المدار !!" });
        //        }


        //        var NIDExist = db.CandidateRepresentatives.Where(x => x.Nid == candidateRepresentatives.Nid).SingleOrDefault();
        //        if (NIDExist != null)
        //            return BadRequest(new { message = "الرقم الوطني موجود مسبقا الرجاء إعادة الادخال" });

        //        var PhoneExist = db.CandidateRepresentatives.Where(x => x.Phone == candidateRepresentatives.Phone).SingleOrDefault();
        //        if (PhoneExist != null)
        //            return BadRequest(new { message = "رقم الهاتف موجود مسبقا الرجاء إعادة الادخال" });

        //        var EmailExist = db.CandidateRepresentatives.Where(x => x.Email == candidateRepresentatives.Email).SingleOrDefault();
        //        if (EmailExist != null)
        //            return BadRequest(new { message = "البريد الإلكتروني موجود مسبقا الرجاء إعادة الادخال" });

        //        if (string.IsNullOrWhiteSpace(candidateRepresentatives.BirthDate.ToString()))
        //        {
        //            return BadRequest(new { message = "الرجاء دخال تاريخ الميلاد "});
        //        }
        //        if ((DateTime.Now.Year - candidateRepresentatives.BirthDate.GetValueOrDefault().Year) < 18)
        //        {
        //            return BadRequest(new { message = "يجب ان يكون عمر الممثل اكبر من 18"});
        //        }


        //        candidateRepresentatives.CreatedBy = userId;
        //        candidateRepresentatives.CreatedOn = DateTime.Now;
        //        db.CandidateRepresentatives.Add(candidateRepresentatives);
        //        db.SaveChanges();

        //        return Ok(new { message = " تم اضافة الممثل  بنـجاح" });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}
        public class CandidateRepresentative
        {
            public CandidateRepresentatives candidateRepresentatives { get; set; }
            public string candidateNid { get; set; }
        }
        [HttpPost("Candidate/Add")]
        public IActionResult AddCandidateRepresentative([FromBody] CandidateRepresentative candidateRepresentative)
        {
            try
            {
                var profile = db.Profile.Where(x => x.Status == 1).FirstOrDefault();

                if(profile == null)
                {
                    return BadRequest(new { message =" لا يوجد ملف إنتخابي مفعل"});
                }

                if (candidateRepresentative == null)
                {
                    return BadRequest(new { message = "حذث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" });
                }
                var candidate = db.Candidates.Where(x => x.Nid == candidateRepresentative.candidateNid).FirstOrDefault();
                candidateRepresentative.candidateRepresentatives.CandidateId = candidate.CandidateId;


                if (candidateRepresentative.candidateRepresentatives.CandidateId == 0 || candidateRepresentative.candidateRepresentatives.CandidateId == null)
                {
                    return StatusCode(404, new { message = "الرجاء إختيار المرشح" });
                }

                if (string.IsNullOrEmpty(candidateRepresentative.candidateRepresentatives.Nid))
                {
                    return StatusCode(404, new { message = "الرجاء إدخال الرقم الوطني" });
                }

                if (candidateRepresentative.candidateRepresentatives.Nid.Trim().Length != 12)
                {
                    return StatusCode(404, new { message = "يجب أن يكون طول الرقم الوطني 12 رقماً" });
                }

                if (string.IsNullOrWhiteSpace(candidateRepresentative.candidateRepresentatives.Email))
                {
                    return StatusCode(404, new { message = "الرجاء إدخال البريد الالكتروني" });
                }
                if (!Common.Validation.IsValidEmail(candidateRepresentative.candidateRepresentatives.Email))
                {
                    return StatusCode(404, new { message = "الرجاء التأكد من البريد الإلكتروني" });
                }
                if (string.IsNullOrWhiteSpace(candidateRepresentative.candidateRepresentatives.Phone))
                {
                    return StatusCode(404, new { message = "الرجاء إدخال رقم الهاتف" });
                }
                if (candidateRepresentative.candidateRepresentatives.Phone.Length < 9)
                {
                    return StatusCode(404, new { message = "الرجاء إدخال الهـاتف بطريقة الصحيحة !!" });
                }
                candidateRepresentative.candidateRepresentatives.Phone = candidateRepresentative.candidateRepresentatives.Phone.Substring(candidateRepresentative.candidateRepresentatives.Phone.Length - 9);

                if (candidateRepresentative.candidateRepresentatives.Phone.Substring(0, 2) != "91" && candidateRepresentative.candidateRepresentatives.Phone.Substring(0, 2) != "92" && candidateRepresentative.candidateRepresentatives.Phone.Substring(0, 2) != "94" && candidateRepresentative.candidateRepresentatives.Phone.Substring(0, 2) != "93" && candidateRepresentative.candidateRepresentatives.Phone.Substring(0, 2) != "95")
                {
                    return StatusCode(404, new { message = "يجب ان يكون الهاتف يبدأ ب (91,92,93,94,95)   ليبيانا او المدار !!" });
                }


                var NIDExist = db.CandidateRepresentatives.Where(x => x.Nid == candidateRepresentative.candidateRepresentatives.Nid).SingleOrDefault();
                if (NIDExist != null)
                    return BadRequest(new { message = "الرقم الوطني موجود مسبقا الرجاء إعادة الادخال" });

                var PhoneExist = db.CandidateRepresentatives.Where(x => x.Phone == candidateRepresentative.candidateRepresentatives.Phone).SingleOrDefault();
                if (PhoneExist != null)
                    return BadRequest(new { message = "رقم الهاتف موجود مسبقا الرجاء إعادة الادخال" });

                var EmailExist = db.CandidateRepresentatives.Where(x => x.Email == candidateRepresentative.candidateRepresentatives.Email).SingleOrDefault();
                if (EmailExist != null)
                    return BadRequest(new { message = "البريد الإلكتروني موجود مسبقا الرجاء إعادة الادخال" });

                if (string.IsNullOrWhiteSpace(candidateRepresentative.candidateRepresentatives.BirthDate.ToString()))
                {
                    return BadRequest(new { message = "الرجاء دخال تاريخ الميلاد " });
                }
                if ((DateTime.Now.Year - candidateRepresentative.candidateRepresentatives.BirthDate.GetValueOrDefault().Year) < 18)
                {
                    return BadRequest(new { message = "يجب ان يكون عمر الممثل اكبر من 18" });
                }


                candidateRepresentative.candidateRepresentatives.CreatedBy = userId;
                candidateRepresentative.candidateRepresentatives.CreatedOn = DateTime.Now;
                
                db.CandidateRepresentatives.Add(candidateRepresentative.candidateRepresentatives);

                
                candidate.Levels = 5;

                db.Candidates.Update(candidate);
                db.SaveChanges();

                return Ok(new { message = " تم اضافة الممثل  بنـجاح", level = candidate.Levels });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("GetRepresentativesBy/{candidateId}")]
        public IActionResult GetCandidateRepresentatives([FromRoute] long? candidateId)
        {
            try
            {
                if(candidateId == 0 || candidateId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المرشح"});
                }

                var representatives = db.CandidateRepresentatives.Where(x => x.CandidateId == candidateId).Select(x => new {
                    Name = string.Format("{0} {1} {2} {3}", x.FirstName, x.FatherName, x.GrandFatherName, x.SurName),
                    x.Nid,
                    x.MotherName,
                    x.Phone,
                    x.Gender,
                    x.Email,
                    x.CreatedOn,
                    x.BirthDate,
                }).ToList();
                return Ok(new { representatives });
            }
            catch(Exception e)
            {
                return BadRequest(new { message = e.InnerException.Message});
            }
        }








    }
}