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
using Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using NodaTime;
using static Services.Helper;

namespace Dashboard.Controllers
{
    [Produces("application/json")]
    [ValidateAntiForgeryToken]
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

        [HttpGet("{id}/Entity")]
        public IActionResult GetCandidatesByEntityId([FromRoute]long? id)
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

                IQueryable<Candidates> CandidatesQuery;
                CandidatesQuery = from p in db.Candidates where p.EntityId==id && p.ProfileId == UP.ProfileId && p.Status!=9
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
                                          Name = string.Format("{0} {1} {2} {3}", p.FirstName, p.FatherName, p.GrandFatherName, p.SurName),
                                          subconstituencyName = sc.ArabicName,
                                          p.Nid,
                                          p.CreatedOn

                                      }).ToList();

                return Ok(new { Candidates = CandidatesList, count = CandidatesCount });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "حدث خطاء، حاول مجدداً" );
            }

        }

        [HttpGet("Get/{CandidateId}")]
        public IActionResult GetCandidate([FromRoute]long? CandidateId)
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

                if (CandidateId == null)
                {
                    return BadRequest("حدث خطأ في إستقبال البيانات الرجاء إعادة الادخال" );
                }

                var candidate = db.Candidates.Where(x => x.CandidateId == CandidateId && x.ProfileId == UP.ProfileId)
                    .Select(s => new { s.FirstName, s.FatherName, s.GrandFatherName, s.SurName, s.MotherName, s.BirthDate, s.CompetitionType, s.ConstituencyId, s.SubConstituencyId, s.Email, s.Gender, s.Qualification, s.HomePhone, s.CandidateId }).FirstOrDefault();

                if (candidate == null)
                {
                    return BadRequest( "لا يوجد ناخب مسجل ");
                }
                var constituency = db.Constituencies.Where(x => x.ConstituencyId == candidate.ConstituencyId).SingleOrDefault();
                var Region = db.Regions.Where(x => x.RegionId == constituency.RegionId).FirstOrDefault();

                return Ok(new { candidate, Region.RegionId});
               
            }
            catch (Exception ex)
            {
                return StatusCode(500,"حدث خطاء، حاول مجدداً");
            }

        }


        [HttpGet("Get")]
        public IActionResult GetCandidates([FromQuery]int pageNo, [FromQuery]int pageSize,long SubConstituencyId)
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

                IQueryable<Candidates> CandidatesQuery;
                if (SubConstituencyId <= 0)
                {
         
                    CandidatesQuery = from p in db.Candidates
                                      where p.Status != 9 && p.ProfileId == UP.ProfileId
                                      select p;
                } else
                {
                    
                    CandidatesQuery = from p in db.Candidates
                                      where p.Status != 9 && p.ProfileId == UP.ProfileId && p.SubConstituencyId == SubConstituencyId
                                      select p;
                }
               

                var CandidatesCount = (from p in CandidatesQuery
                                       select p).Count();

                var CandidatesList = (from p in CandidatesQuery
                                      join sc in db.ConstituencyDetails on p.SubConstituencyId equals sc.ConstituencyDetailId
                                      orderby p.CreatedOn descending
                                      select new
                                      {
                                          p.CandidateId,
                                          p.Levels,
                                          Name = string.Format("{0} {1} {2} {3}", p.FirstName, p.FatherName, p.GrandFatherName, p.SurName),
                                          subconstituencyName = sc.ArabicName,
                                          p.Nid,
                                          p.CreatedOn
                                      }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Candidates = CandidatesList, count = CandidatesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpGet("NationalId/{NationalId}")]
        public IActionResult GetNationalIdBasedOn([FromRoute] string nationalId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }
                if (string.IsNullOrEmpty(nationalId) || string.IsNullOrWhiteSpace(nationalId))
                {
                    return BadRequest("الرجاء التأكد من إدخال الرقم الوطني");
                }

                //if (!int.TryParse(nationalId.Trim(), out _))
                //{
                //    return BadRequest(new { message = "الرقم الوطني يجب يتكون من أرقام فقط" });
                //}

                if (nationalId.Length != 12)
                {
                    return BadRequest("الرقم الوطني يتكون من 12 رقماً");
                }

                var candidate = db.Candidates.Where(x => x.Nid == nationalId && x.ProfileId== UP.ProfileId).FirstOrDefault();

                if (candidate == null)
                {
                    candidate = new Candidates
                    {
                        Levels = 1,
                        Nid = nationalId,
                        ProfileId = UP.ProfileId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = UP.UserId
                    };
                    db.Candidates.Add(candidate);
                    db.SaveChanges();
                    return Ok(new { level = candidate.Levels, candidate.Nid });
                }
                else
                {
                    return Ok(new { level = candidate.Levels, candidate.Nid });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("SendVerifyCode")]
        public IActionResult RegisterPhone([FromBody] Candidates candidates)
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
                string codePhoneNumber = candidates.Phone.Substring(0, 3);
                if (!codePhoneNumber.Equals("091") && !codePhoneNumber.Equals("092") && !codePhoneNumber.Equals("093") && !codePhoneNumber.Equals("094"))
                {
                    return BadRequest("يجب أن يبدأ الرقم ب091 أو 092 أو 093 أو 094" );
                }

                if (candidates.Phone.Length != 10)
                {
                    return BadRequest("يجب أن يكون رقم الهاتف بطول 10 أرقام" );
                }

                var phoneCount = db.Candidates.Where(x => x.Phone == candidates.Phone && x.ProfileId == UP.ProfileId).Count();

                if (phoneCount > 0)
                {
                    return BadRequest("لقد تم إستخدام هذا الرقم بالفعل من قبل مرشح أخر" );
                }
                return Ok(new { isVerifyCodeSent = true, message = "الرجاء إدخال رمز التحقق" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "حدث خطاء، حاول مجدداً");
            }
        }

        public class Verification
        {
            public int? VerifyCode { get; set; }
            public string Nid { get; set; }
            public string Phone { get; set; }
        }

        public class File
        {
            public string Nid { get; set; }
            public IFormFile[] fileList { get; set; }
        }


        [HttpPost("VerifyPhone")]
        public IActionResult VerifyPhone([FromBody] Verification obj)
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

                if (obj.VerifyCode != 1111)
                {
                    return BadRequest("رمز التحقق الذي أدخلته خطأ، أعد المحاولة");
                }

                var candidate = db.Candidates.Where(x => x.Nid == obj.Nid && x.ProfileId == UP.ProfileId).FirstOrDefault();

                if (candidate == null)
                {

                    return BadRequest( string.Format("لا يوجد ناخب مسجل تحت الرقم الوطني {0}", obj.Nid));

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
                return StatusCode(500, "حدث خطاء، حاول مجدداً" );
            }
        }

        [HttpPost("Data")]
        public IActionResult CreateCandidate([FromBody] Candidates candidates)
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


                var Profile = db.Profile.Where(x =>  x.ProfileId == UP.ProfileId).SingleOrDefault();
                if (Profile == null)
                {
                    return StatusCode(404, "الرجاء تفعيل الضبط الانتخابي");
                }

                if (candidates == null)
                {
                    return BadRequest("حدث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var candidate = db.Candidates.Where(x => x.Nid == candidates.Nid).FirstOrDefault();

                if (candidate == null)
                {

                    return BadRequest(string.Format("لا يوجد ناخب مسجل تحت الرقم الوطني {0}", candidates.Nid));

                }



               

                if (string.IsNullOrEmpty(candidates.FirstName) || string.IsNullOrWhiteSpace(candidates.FirstName))
                {
                    return BadRequest("الرجاء إدخال اسم الأول" );
                }

                if (string.IsNullOrEmpty(candidates.FatherName) || string.IsNullOrWhiteSpace(candidates.FatherName))
                {
                    return BadRequest( "الرجاء إدخال اسم الأب" );
                }

                if (string.IsNullOrEmpty(candidates.GrandFatherName) || string.IsNullOrWhiteSpace(candidates.GrandFatherName))
                {
                    return BadRequest( "الرجاء إدخال اسم الجد" );
                }

                if (string.IsNullOrEmpty(candidates.SurName) || string.IsNullOrWhiteSpace(candidates.SurName))
                {
                    return BadRequest("الرجاء إدخال القب" );
                }

                if (string.IsNullOrEmpty(candidates.MotherName) || string.IsNullOrWhiteSpace(candidates.MotherName))
                {
                    return BadRequest("الرجاء إدخال إسم الأم الثلاثي" );
                }



                if (candidates.Gender != 1 && candidates.Gender != 2)
                {
                    return BadRequest( "الرجاء إختيار الجنس" );
                }



                if (candidates.ConstituencyId == 0 || candidates.ConstituencyId == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الرئيسية");
                }

                if (candidates.SubConstituencyId == 0 || candidates.SubConstituencyId == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الفرعية");
                }



                if (string.IsNullOrEmpty(candidates.Email) || string.IsNullOrWhiteSpace(candidates.Email))
                {
                    return BadRequest("الرجاء إدخال البريد الإلكتروني" );
                }

                if (!IsItValidEmail(candidates.Email))
                {
                    return BadRequest("الرجاء إدخال البريد الإلكتروني بشكل صحيح");

                }

                LocalDate birthday = LocalDateTime.FromDateTime(candidates.BirthDate.Value).Date; // For example
                LocalDate today = LocalDateTime.FromDateTime(DateTime.Now).Date; // See below
                Period period = Period.Between(birthday, today);


                if (period.Years < Profile.Age)
                {
                    return BadRequest( string.Format("يجب أن يكون عمر المترشح على الأقل {0}", Profile.Age));
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
                candidate.ProfileId = UP.ProfileId;
                

                db.Candidates.Update(candidate);
                db.SaveChanges();

                return Ok(new { level = candidate.Levels, message = "تم تسجيل بيانات المرشح بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "حدث خطاء، حاول مجدداً" );
            }
        }

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

        [HttpPost("Edit")]
        public IActionResult UpdateCandidate([FromBody] Candidates candidates)
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
                    return BadRequest("حدث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var candidate = db.Candidates.Where(x => x.CandidateId == candidates.CandidateId).FirstOrDefault();

                if (candidate == null)
                {

                    return BadRequest(string.Format("لا يوجد ناخب مسجل تحت الرقم الوطني {0}", candidates.Nid));

                }

                if (string.IsNullOrEmpty(candidates.FirstName) || string.IsNullOrWhiteSpace(candidates.FirstName))
                {
                    return BadRequest("الرجاء إدخال اسم الأول");
                }

                if (string.IsNullOrEmpty(candidates.FatherName) || string.IsNullOrWhiteSpace(candidates.FatherName))
                {
                    return BadRequest("الرجاء إدخال اسم الأب" );
                }

                if (string.IsNullOrEmpty(candidates.GrandFatherName) || string.IsNullOrWhiteSpace(candidates.GrandFatherName))
                {
                    return BadRequest("الرجاء إدخال اسم الجد" );
                }

                if (string.IsNullOrEmpty(candidates.SurName) || string.IsNullOrWhiteSpace(candidates.SurName))
                {
                    return BadRequest("الرجاء إدخال القب" );
                }

                if (string.IsNullOrEmpty(candidates.MotherName) || string.IsNullOrWhiteSpace(candidates.MotherName))
                {
                    return BadRequest("الرجاء إدخال إسم الأم الثلاثي");
                }



                if (candidates.Gender != 1 && candidates.Gender != 2)
                {
                    return BadRequest( "الرجاء إختيار الجنس" );
                }



                if (candidates.ConstituencyId == 0 || candidates.ConstituencyId == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الرئيسية");
                }

                if (candidates.SubConstituencyId == 0 || candidates.SubConstituencyId == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الفرعية" );
                }


                if (string.IsNullOrEmpty(candidates.Email) || string.IsNullOrWhiteSpace(candidates.Email))
                {
                    return BadRequest( "الرجاء إدخال البريد الإلكتروني" );
                }

                if (!IsItValidEmail(candidates.Email))
                {
                    return BadRequest("الرجاء إدخال البريد الإلكتروني بشكل صحيح");
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
               
                //candidate.Levels = 3;


                db.Candidates.Update(candidate);
                db.SaveChanges();


                //bool IsItValidEmail(string emailaddress)
                //{
                //    try
                //    {
                //        var email = new MailAddress(emailaddress);

                //        return true;
                //    }
                //    catch (FormatException)
                //    {
                //        return false;
                //    }
                //}

                return Ok( "تم تحديث المرشح بنجاح" );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "حدث خطاء، حاول مجدداً");
            }
        }

        [HttpPost("UploadDocuments")]
        public IActionResult UploadDocuments([FromBody]File file)
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

               if (file == null)
                {
                    return BadRequest( "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }
                var candidate = db.Candidates.Where(x => x.Nid == file.Nid && x.ProfileId == UP.ProfileId).FirstOrDefault();


                var path = Environment.CurrentDirectory;
                var fullPath = Directory.CreateDirectory(path + "/Documents/" + candidate.CandidateId);

                if (file.fileList.Length > 0)
                {
                    using (FileStream fileStream = System.IO.File.Create(fullPath + "/" + file.fileList[0].FileName))
                    {
                        file.fileList[0].CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }



                return Ok(new { path });
            }
            catch (Exception ex)
            {

                return StatusCode(500, "حدث خطاء، حاول مجدداً");
            }
        }


        private bool CandidatesExists(long id)
        {
            return db.Candidates.Any(e => e.CandidateId == id);
        }

        public class CandidateDocument
        {
            public string Nid { get; set; }
            public string BirthCertificateDocument { get; set; }
            public string NidDocument { get; set; }
            public string FamilyPaper{ get; set; }
            public string AbsenceOfPrecedents{ get; set; }
            public string PaymentReceipt{ get; set; }
        }

        [HttpPost("Attachments")]

        public IActionResult AddCandidateAttachments([FromBody] CandidateDocument candidateDocument)
        {
            try
            {
                if (candidateDocument == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }

                if (string.IsNullOrEmpty(candidateDocument.BirthCertificateDocument))
                {
                    return BadRequest("الرجاء ادخال شهادة الميلاد");
                }
                if (string.IsNullOrEmpty(candidateDocument.NidDocument))
                {
                    return BadRequest("الرجاء ادخال وئيقة الرقم الوطني");
                }
                if (string.IsNullOrEmpty(candidateDocument.FamilyPaper))
                {
                    return BadRequest("الرجاء ادخال ورقة العائلة");
                }
                if (string.IsNullOrEmpty(candidateDocument.AbsenceOfPrecedents))
                {
                    return BadRequest("الرجاء ادخال شهادة الخلو من السوابق");
                }
                if (string.IsNullOrEmpty(candidateDocument.PaymentReceipt))
                {
                    return BadRequest("الرجاء ادخال إيصال الدفع");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }


                var candidate = db.Candidates.Where(x => x.Nid == candidateDocument.Nid && x.ProfileId == UP.ProfileId).FirstOrDefault();

                if(candidate == null)
                {
                    return BadRequest("المرشح غير موجود");
                }

                byte[] birthCertificateDocument;
                birthCertificateDocument = Convert.FromBase64String(candidateDocument.BirthCertificateDocument.Substring(candidateDocument.BirthCertificateDocument.IndexOf(",") + 1));

                byte[] NidDocument;
                NidDocument = Convert.FromBase64String(candidateDocument.NidDocument.Substring(candidateDocument.NidDocument.IndexOf(",") + 1));

                byte[] FamilyPaper;
                FamilyPaper = Convert.FromBase64String(candidateDocument.FamilyPaper.Substring(candidateDocument.FamilyPaper.IndexOf(",") + 1));

                byte[] AbsenceOfPrecedents;
                AbsenceOfPrecedents = Convert.FromBase64String(candidateDocument.AbsenceOfPrecedents.Substring(candidateDocument.AbsenceOfPrecedents.IndexOf(",") + 1));

                byte[] PaymentReceipt;
                PaymentReceipt = Convert.FromBase64String(candidateDocument.PaymentReceipt.Substring(candidateDocument.PaymentReceipt.IndexOf(",") + 1));

                string subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Attachments", candidate.CandidateId.ToString());

                //string birthCertificateDocumentFileName = @"Section" + Guid.NewGuid() + ".pdf";
                List<string> filesName = new List<string> {
                    @"BirthCertificate" + Guid.NewGuid() + ".pdf",
                    @"Nid" + Guid.NewGuid() + ".pdf",
                    @"FamilyPaper" + Guid.NewGuid() + ".pdf",
                    @"AbsenceOfPrecedents" + Guid.NewGuid() + ".pdf",
                    @"PaymentReceipt" + Guid.NewGuid() + ".pdf",
                 };
                //string birthCertificateDirectory = "/PDF/" + candidate.CandidateId.ToString() + "/" + birthCertificateDocumentFileName;

                var filesDirectories = new Dictionary<string, string>
                {
                    { "BirthCertificate", "/Attachments/" + candidate.CandidateId.ToString() + "/" + filesName[0]},
                    { "Nid", "/Attachments/" + candidate.CandidateId.ToString() + "/" + filesName[1]},
                    { "FamilyPaper", "/Attachments/" + candidate.CandidateId.ToString() + "/" + filesName[2]},
                    { "AbsenceOfPrecedents", "/Attachments/" + candidate.CandidateId.ToString() + "/" + filesName[3]},
                    { "PaymentReceipt", "/Attachments/" + candidate.CandidateId.ToString() + "/" + filesName[4]},
                };

                bool exists = System.IO.Directory.Exists(subPath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(subPath);

                MemoryStream [] streams = { new MemoryStream(birthCertificateDocument), new MemoryStream(NidDocument), new MemoryStream(FamilyPaper), new MemoryStream(AbsenceOfPrecedents), new MemoryStream(PaymentReceipt) };

                for(var i = 0; i < streams.Length; i++)
                {
                    IFormFile file = new FormFile(streams[i], 0, streams[i].Length, "وثائق الناحب", filesName[i]);

                    string fullpath = Path.Combine(subPath, file.FileName);
                    using (var fileStream = new FileStream(fullpath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
               
               

                candidate.Levels = 4;
                db.Candidates.Update(candidate);

                var attachments = new CandidateAttachments();
                attachments.BirthDateCertificate = filesDirectories["BirthCertificate"];
                attachments.CandidateId = candidate.CandidateId;
                attachments.Nidcertificate = filesDirectories["Nid"];
                attachments.FamilyPaper = filesDirectories["FamilyPaper"];
                attachments.AbsenceOfPrecedents = filesDirectories["AbsenceOfPrecedents"];
                attachments.PaymentReceipt = filesDirectories["PaymentReceipt"];
                attachments.CandidateId = candidate.CandidateId;
                attachments.CreatedBy = userId;
                attachments.CreatedOn = DateTime.Now;
                attachments.Status = 1;
                db.CandidateAttachments.Add(attachments);

                candidate.Levels = 4;
              
                db.Candidates.Update(candidate);
                db.SaveChanges();
                return Ok(new { message = "لقد قمت برفع الملفات بنــجاح", level = candidate.Levels});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet]
        public IActionResult GetCandidateIdByNationalId([FromQuery] string nationalId)
        {
            try
            {
                if (string.IsNullOrEmpty(nationalId))
                {
                    return BadRequest("الرجاء قم بإدخال الرقم الوطني");
                }

                var candidate = db.Candidates.Where(x => x.Nid == nationalId).Select(x => new { x.FirstName, x.FatherName, x.GrandFatherName, x.SurName, x.Levels, x.CandidateId }).SingleOrDefault();

               
                return Ok(new { candidateId = candidate.CandidateId, candidateName = string.Format("{0} {1} {2} {3}", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName) });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message );
            }
        }

        [HttpGet("Complete/{NationalId}")]
        public IActionResult CompleteRegistration([FromRoute] string NationalId)
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

    
                var candidate = (from c in db.Candidates join sc in db.ConstituencyDetails on c.SubConstituencyId equals sc.ConstituencyDetailId where c.Nid == NationalId select new { c.Nid, fullName = string.Format("{0} {1} {2}", c.FirstName, c.FatherName, c.SurName), subconstituencyName = sc.ArabicName }).FirstOrDefault();
                return Ok(candidate);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("AddCandidateToEntity")]
        public IActionResult AddCandidateToEntity([FromBody] Candidates candidates)
        {
            try
            {
                if (candidates == null)
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");

                if (string.IsNullOrEmpty(candidates.Nid))
                    return StatusCode(404, "الرجاء إدخال الرقم الوطني");

                if (candidates.Nid.Length < 12 || candidates.Nid.Length > 13)
                    return StatusCode(404, "الرجاء إدخال الرقم الوطني بطريقة الصحيحه");

                var Candidate = db.Candidates.Where(x => x.Nid == candidates.Nid && x.Status != 9).SingleOrDefault();

                if (Candidate==null)
                    return StatusCode(404, "لم يتم العتور علي المرشح");

                //IsExistInEntity
                if (Candidate.EntityId!=null)
                    return StatusCode(404, "المرشح موجود في كيان اخر الرجاء التحقق من البيانات");

                Candidate.EntityId = candidates.EntityId;

                db.SaveChanges();

                return Ok(" تم اضافة المرشح للكيان  بنـجاح");
            }
            catch (Exception ex)
            {
                return StatusCode(500,  "حدث خطاء، حاول مجدداً" );
            }
        }

        [HttpPost("Add/User")]
        public IActionResult AddCandidateUser([FromBody] CandidateUsers user)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (string.IsNullOrWhiteSpace(user.LoginName))
                {
                    return BadRequest("الرجاء ادحال اسم المسنخدم بطريقة صحيحة");
                }

                if (string.IsNullOrWhiteSpace(user.Name))
                {
                    return BadRequest("الرجاء إدخال الاسم الرباعي");
                }

                if (!Validation.IsValidEmail(user.Email))
                {
                    return BadRequest("الرجاء ادخال الايميل بالطريقة الصحيحة");
                }

                if (user.Gender != 1 && user.Gender != 2)
                {
                    return BadRequest("الرجاء ادخال الجنس (ذكر - انثي)");

                }
                if (string.IsNullOrWhiteSpace(user.BirthDate.ToString()))
                {
                    return BadRequest("الرجاء دخال تاريخ الميلاد المستخدم");
                }
                if ((DateTime.Now.Year - user.BirthDate.GetValueOrDefault().Year) < 18)
                {
                    return BadRequest("يجب ان يكون عمر المستخدم اكبر من 18");
                }

                var cLoginName = (from u in db.CandidateUsers
                                  where u.LoginName == user.LoginName
                                  select u).SingleOrDefault();
                if (cLoginName != null)
                {
                    return BadRequest(" اسم الدخول موجود مسبقا");


                }


                var cPhone = (from u in db.CandidateUsers
                              where u.Phone == user.Phone
                              select u).SingleOrDefault();
                if (cPhone != null)
                {
                    return BadRequest(" رقم الهاتف موجود مسبقا");
                }

                var cUser = (from u in db.CandidateUsers
                             where u.Email == user.Email && u.Status != 9
                             select u).SingleOrDefault();

                if (cUser != null)
                {
                    if (cUser.Status == 0)
                    {
                        return BadRequest("هدا المستخدم موجود من قبل يحتاج الي تقعيل الحساب فقط");
                    }
                    if (cUser.Status == 1 || cUser.Status == 2)
                    {
                        return BadRequest("هدا المستخدم موجود من قبل يحتاج الي دخول فقط");
                    }
                }

                cUser = new CandidateUsers();


                cUser.Phone = user.Phone;
                cUser.LoginName = user.LoginName;
                cUser.Name = user.Name;
                cUser.Email = user.Email;
                cUser.BirthDate = user.BirthDate;
                cUser.CreatedBy = userId;
                cUser.CreatedOn = DateTime.Now;
                cUser.Gender = (short)user.Gender;
                cUser.LoginTryAttempts = 0;
                cUser.UserType = user.UserType;
                cUser.CandidateId = user.CandidateId;
                cUser.Password = Security.ComputeHash(user.Password, HashAlgorithms.SHA512, null);
                if (user.Image == null)
                {
                    cUser.Image = Convert.
                        FromBase64String("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////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAADhCSU0EAAAAAAAAAgAAOEJJTQQCAAAAAAACAAA4QklNBDAAAAAAAAEBADhCSU0ELQAAAAAABgABAAAAAjhCSU0ECAAAAAAAEAAAAAEAAAJAAAACQAAAAAA4QklNBB4AAAAAAAQAAAAAOEJJTQQaAAAAAANJAAAABgAAAAAAAAAAAAAAjAAAAIwAAAAKAFUAbgB0AGkAdABsAGUAZAAtADEAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAIwAAACMAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAEAAAAAAABudWxsAAAAAgAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAACMAAAAAFJnaHRsb25nAAAAjAAAAAZzbGljZXNWbExzAAAAAU9iamMAAAABAAAAAAAFc2xpY2UAAAASAAAAB3NsaWNlSURsb25nAAAAAAAAAAdncm91cElEbG9uZwAAAAAAAAAGb3JpZ2luZW51bQAAAAxFU2xpY2VPcmlnaW4AAAANYXV0b0dlbmVyYXRlZAAAAABUeXBlZW51bQAAAApFU2xpY2VUeXBlAAAAAEltZyAAAAAGYm91bmRzT2JqYwAAAAEAAAAAAABSY3QxAAAABAAAAABUb3AgbG9uZwAAAAAAAAAATGVmdGxvbmcAAAAAAAAAAEJ0b21sb25nAAAAjAAAAABSZ2h0bG9uZwAAAIwAAAADdXJsVEVYVAAAAAEAAAAAAABudWxsVEVYVAAAAAEAAAAAAABNc2dlVEVYVAAAAAEAAAAAAAZhbHRUYWdURVhUAAAAAQAAAAAADmNlbGxUZXh0SXNIVE1MYm9vbAEAAAAIY2VsbFRleHRURVhUAAAAAQAAAAAACWhvcnpBbGlnbmVudW0AAAAPRVNsaWNlSG9yekFsaWduAAAAB2RlZmF1bHQAAAAJdmVydEFsaWduZW51bQAA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IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTExIDc5LjE1ODMyNSwgMjAxNS8wOS8xMC0wMToxMDoyMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgeG1wOk1vZGlmeURhdGU9IjIwMTctMTItMDFUMTk6MzQ6MTcrMDI6MDAiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MmE0MzNlNTUtNzk5ZC00NTRlLWI1ZTUtYWIwNjFmOTUwNThhIiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZDM3ZThiZTYtZDZiZC0xMWU3LWIxNWEtOTViY2JlMzViMTFhIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZjMzZTk0OWItYWNkYi04MjQxLWIxNTctNDgwNDEyMDdkMzNmIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSI+IDx4bXBNTTpIaXN0b3J5PiA8cmRmOlNlcT4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNyZWF0ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6ZjMzZTk0OWItYWNkYi04MjQxLWIxNTctNDgwNDEyMDdkMzNmIiBzdEV2dDp3aGVuPSIyMDE3LTEyLTAxVDE5OjM0OjE3KzAyOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cykiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjJhNDMzZTU1LTc5OWQtNDU0ZS1iNWU1LWFiMDYxZjk1MDU4YSIgc3RFdnQ6d2hlbj0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDwvcmRmOlNlcT4gPC94bXBNTTpIaXN0b3J5PiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8P3hwYWNrZXQgZW5kPSJ3Ij8+/+IMWElDQ19QUk9GSUxFAAEBAAAMSExpbm8CEAAAbW50clJHQiBYWVogB84AAgAJAAYAMQAAYWNzcE1TRlQAAAAASUVDIHNSR0IAAAAAAAAAAAAAAAEAAPbWAAEAAAAA0y1IUCAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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");
                }
                else
                {
                    cUser.Image = Convert.FromBase64String(user.Image.ToString().Substring(user.Image.ToString().IndexOf(",") + 1));
                }
                cUser.CreatedOn = DateTime.Now;

                //1- Active
                //2- locked
                //9- deleted not exist
                cUser.Status = 1;
                db.CandidateUsers.Add(cUser);

                db.SaveChanges();
                return Ok("تم تسجيل المستخدم بنجاح " );
            }
            catch (Exception e)
            {
                return StatusCode(500,  e.Message );
            }
        }

        [HttpGet("GetUser/{candidateId}")]
        public IActionResult GetCandidateUser([FromRoute]long? candidateId)
        {
            try
            {
                IQueryable<CandidateUsers> Users = from p in db.CandidateUsers
                                                where
                       p.Status != 9 &&
                       p.Status != 6 &&
                       p.CandidateId == candidateId
                                                select p;

                var UsersCount = (from p in Users
                                  select p).Count();


                var UserInfo = (from p in Users
                                orderby p.CreatedOn descending
                                select new
                                {
                                    UserId = p.CandidateUserId,
                                    Name = p.Name,
                                    LoginName = p.LoginName,
                                    State = p.Status,
                                    Email = p.Email,
                                    Password = p.Password,
                                    CreatedOn = p.CreatedOn,
                                    Phone = p.Phone,
                                    gender = p.Gender,
                                    BirthDate = p.BirthDate,
                                    CreatedBy = p.CreatedBy,
                                    Image = p.Image,
                                    UserType = p.UserType
                                }).ToList();

                return Ok(new { users = UserInfo, count = UsersCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("image/{UserId}")]
        public IActionResult GetUserImage([FromRoute]long UserId)
        {
            try
            {
                var UserImage = (from p in db.CandidateUsers
                                 where p.CandidateUserId == UserId
                                 select p.Image).SingleOrDefault();

                if (UserImage == null)
                {
                    return NotFound("المستخدم غير موجــود");
                }

                return File(UserImage, "image/jpeg");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{UserId}/Deactivate")]
        public IActionResult Deactivate(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var User = (from p in db.CandidateUsers
                            where p.CandidateUserId == UserId && p.Status != 9
                            select p).SingleOrDefault();

                if (User == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                User.Status = 2;
                db.SaveChanges();
                return Ok("تم العمليه بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{UserId}/Activate")]
        public IActionResult Activate(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var User = (from p in db.CandidateUsers
                            where p.CandidateUserId == UserId && p.Status != 9
                            select p).SingleOrDefault();

                if (User == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                User.Status = 1;
                db.SaveChanges();
                return Ok("تم العمليه بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{UserId}/deleteUser")]
        public IActionResult deleteUser(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var User = (from p in db.CandidateUsers
                            where p.CandidateUserId == UserId && p.Status != 9
                            select p).SingleOrDefault();

                if (User == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                User.Status = 9;
                db.SaveChanges();
                return Ok("تم العمليه بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}
