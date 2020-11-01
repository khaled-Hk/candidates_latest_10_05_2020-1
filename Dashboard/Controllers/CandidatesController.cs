using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

using System.IO;

namespace Dashboard.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidatesContext db;
        private Helper help;

        public CandidatesController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        [HttpGet("GetCandidates")]
        public IActionResult Get([FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            try
            {
                IQueryable<Candidates> CandidatesQuery;
                CandidatesQuery = from p in db.Candidates
                                      select p;

                var CandidatesCount = (from p in CandidatesQuery
                                           select p).Count();

                var CandidatesList = (from p in CandidatesQuery
                                        join sc in db.ConstituencyDetails on p.SubConstituencyId equals sc.ConstituencyDetailId
                                        orderby p.CreatedOn descending
                                        select new
                                        {
                                            p.CandidateId,
                                            p.Levels,
                                            Name = string.Format("{0} {1} {2} {3}",p.FirstName, p.FatherName, p.GrandFatherName, p.SurName),
                                            subconstituencyName = sc.ArabicName,
                                            p.Nid,
                                            p.CreatedOn

                                        }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Candidates = CandidatesList, count = CandidatesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("NationalId/{NationalId}")]
        public IActionResult GetNationalIdBasedOn([FromRoute] string nationalId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (string.IsNullOrEmpty(nationalId) || string.IsNullOrWhiteSpace(nationalId))
                {
                    return BadRequest(new { message = "الرجاء التأكد من إدخال الرقم الوطني" });
                }

                //if (!int.TryParse(nationalId.Trim(), out _))
                //{
                //    return BadRequest(new { message = "الرقم الوطني يجب يتكون من أرقام فقط" });
                //}

                if (nationalId.Length != 12)
                {
                    return BadRequest(new { message = "الرقم الوطني يتكون من 12 رقماً" });
                }

                var candidate = db.Candidates.Where(x => x.Nid == nationalId).FirstOrDefault();

                if (candidate == null)
                {
                    candidate = new Candidates
                    {
                        Levels = 1,
                        Nid = nationalId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userId
                    };
                    db.Candidates.Add(candidate);
                    db.SaveChanges();
                    return Ok(new { message = "", level = candidate.Levels, candidate.Nid });
                }
                else
                {
                    return Ok(new { level = candidate.Levels, candidate.Nid });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPost("SendVerifyCode")]
        public IActionResult RegisterPhone([FromBody] Candidates candidates)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                string codePhoneNumber = candidates.Phone.Substring(0, 3);
                if (!codePhoneNumber.Equals("091") && !codePhoneNumber.Equals("092") && !codePhoneNumber.Equals("093") && !codePhoneNumber.Equals("094"))
                {
                    return BadRequest(new { message = "يجب أن يبدأ الرقم ب091 أو 092 أو 093 أو 094" });
                }

                if (candidates.Phone.Length != 10)
                {
                    return BadRequest(new { message = "يجب أن يكون رقم الهاتف بطول 10 أرقام" });
                }

                var phoneCount = db.Candidates.Where(x => x.Phone == candidates.Phone).Count();

                if (phoneCount > 0)
                {
                    return BadRequest(new { message = "لقد تم إستخدام هذا الرقم بالفعل من قبل مرشح أخر" });
                }


                return Ok(new { isVerifyCodeSent = true, message = "الرجاء إدخال رمز التحقق" });



            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        public class Verification
        {
            public int? VerifyCode { get; set; }
            public string Nid { get; set; }
            public string Phone { get; set; }
        }

        public class File{
            public string Nid { get; set; }
            public IFormFile[] fileList { get; set; }
        }

        
        [HttpPost("VerifyPhone")]
        public IActionResult VerifyPhone([FromBody] Verification obj)
        {

        try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if(obj.VerifyCode != 1111)
                {
                    return BadRequest("رمز التحقق الذي أدخلته خطأ، أعد المحاولة");
                }

                var candidate = db.Candidates.Where(x => x.Nid == obj.Nid).FirstOrDefault();

                if (candidate == null)
                {

                    return BadRequest(new { message = string.Format("لا يوجد ناخب مسجل تحت الرقم الوطني {0}", obj.Nid) });

                }
                else
                {
                    candidate.Levels = 2;
                    candidate.Phone = obj.Phone;
                    db.Candidates.Update(candidate);
                    db.SaveChanges();
                    return Ok(new { message = "تم تسجيل رقم الهاتف بنجاح", level = candidate.Levels });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPost("CandidateData")]
        public IActionResult CreateCandidate([FromBody] Candidates candidates)
        {
            try
            {

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (candidates == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }

                var candidate = db.Candidates.Where(x => x.Nid == candidates.Nid).FirstOrDefault();

                if (candidate == null)
                {

                    return BadRequest(new { message = string.Format("لا يوجد ناخب مسجل تحت الرقم الوطني {0}", candidates.Nid) });

                }

                if (string.IsNullOrEmpty(candidates.FirstName) || string.IsNullOrWhiteSpace(candidates.FirstName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم الأول" });
                }

                if (string.IsNullOrEmpty(candidates.FatherName) || string.IsNullOrWhiteSpace(candidates.FatherName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم الأب" });
                }

                if (string.IsNullOrEmpty(candidates.GrandFatherName) || string.IsNullOrWhiteSpace(candidates.GrandFatherName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم الجد" });
                }

                if (string.IsNullOrEmpty(candidates.SurName) || string.IsNullOrWhiteSpace(candidates.SurName))
                {
                    return BadRequest(new { message = "الرجاء إدخال القب" });
                }

                if (string.IsNullOrEmpty(candidates.MotherName) || string.IsNullOrWhiteSpace(candidates.MotherName))
                {
                    return BadRequest(new { message = "الرجاء إدخال إسم الأم الثلاثي" });
                }

                

                if (candidates.Gender != 1 && candidates.Gender != 2)
                {
                    return BadRequest(new { message = "الرجاء إختيار الجنس" });
                }

              

                if (candidates.ConstituencyId == 0 || candidates.ConstituencyId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار الدائرة الرئيسية" });
                }

                if (candidates.SubConstituencyId == 0 || candidates.SubConstituencyId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار الدائرة الفرعية" });
                }


                if (string.IsNullOrEmpty(candidates.Email) || string.IsNullOrWhiteSpace(candidates.Email))
                {
                    return BadRequest(new { message = "الرجاء إدخال البريد الإلكتروني" });
                }

                if (!IsItValidEmail(candidates.Email))
                {
                    return BadRequest(new { message = "الرجاء إدخال البريد الإلكتروني بشكل صحيح" });
                }




                candidate.FirstName = candidates.FirstName;
                candidate.FatherName = candidates.FatherName;
                candidate.GrandFatherName = candidates.GrandFatherName;
                candidate.SurName = candidates.SurName;
                candidate.MotherName = candidates.MotherName;
                candidate.Gender = candidates.Gender;
                candidate.BirthDate = candidates.BirthDate;
                candidate.HomePhone = candidates.HomePhone;
                candidate.Email = candidates.Email;
                candidate.Qualification = candidates.Qualification;
                candidate.ConstituencyId = candidates.ConstituencyId;
                candidate.SubConstituencyId = candidates.SubConstituencyId;
                candidate.CompetitionType = candidates.CompetitionType;
                candidate.Levels = 3;

                db.Candidates.Update(candidate);
                db.SaveChanges();


                bool IsItValidEmail(string emailaddress)
                {
                    try
                    {
                        var email = new MailAddress(emailaddress);

                        return true;
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                }

                return Ok(new {level = candidate.Levels, message = "تم تسجيل بيانات المرشح بنجاح" }) ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPost("UploadDocuments")]
        public IActionResult UploadDocuments([FromBody]File file)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (file == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }
                var candidate = db.Candidates.Where(x => x.Nid == file.Nid).FirstOrDefault();


                var path = Environment.CurrentDirectory;
                var fullPath = Directory.CreateDirectory(path+"/Documents/"+candidate.CandidateId);

                if(file.fileList.Length > 0)
                {
                    using(FileStream fileStream = System.IO.File.Create(fullPath+"/"+file.fileList[0].FileName))
                    {
                        file.fileList[0].CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }

               

                return Ok(new { path });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }


        private bool CandidatesExists(long id)
        {
            return db.Candidates.Any(e => e.CandidateId == id);
        }
    }
}
