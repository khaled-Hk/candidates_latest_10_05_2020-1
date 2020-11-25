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

        [HttpGet("GetCandidate/{CandidateId}")]
        public IActionResult GetCandidate([FromRoute]long? CandidateId)
        {
            try
            {
                if (CandidateId == null)
                {
                    return BadRequest(new { message = "حدث خطأ في إستقبال البيانات الرجاء إعادة الادخال" });
                }

                var candidate = db.Candidates.Where(x => x.CandidateId == CandidateId).Select(s => new { s.FirstName, s.FatherName, s.GrandFatherName, s.SurName, s.MotherName, s.BirthDate, s.CompetitionType, s.ConstituencyId, s.SubConstituencyId, s.Email, s.Gender, s.Qualification, s.HomePhone, s.CandidateId }).FirstOrDefault();

                if (candidate == null)
                {
                    return BadRequest(new { message = "لا يوجد ناخب مسجل " });
                }

                return Ok(new { candidate });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }

        }


        [HttpGet("GetCandidates")]
        public IActionResult GetCandidates([FromQuery]int pageNo, [FromQuery]int pageSize)
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
                                          Name = string.Format("{0} {1} {2} {3}", p.FirstName, p.FatherName, p.GrandFatherName, p.SurName),
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
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (obj.VerifyCode != 1111)
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

                return Ok(new { level = candidate.Levels, message = "تم تسجيل بيانات المرشح بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPut("UpdateCandidate")]
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
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }

                var candidate = db.Candidates.Where(x => x.CandidateId == candidates.CandidateId).FirstOrDefault();

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

                return Ok(new { level = candidate.Levels, message = "تم تحديث المرشح بنجاح" });
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

                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
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

        [HttpPost("CandidateAttachments")]

        public IActionResult AddCandidateAttachments([FromBody] CandidateDocument candidateDocument)
        {
            try
            {
                if (candidateDocument == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
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


                var candidate = db.Candidates.Where(x => x.Nid == candidateDocument.Nid).SingleOrDefault();

                byte[] birthCertificateDocument;
                birthCertificateDocument = Convert.FromBase64String(candidateDocument.BirthCertificateDocument.Substring(candidateDocument.BirthCertificateDocument.IndexOf(",") + 1));

                byte[] NidDocument;
                NidDocument = Convert.FromBase64String(candidateDocument.NidDocument.Substring(candidateDocument.BirthCertificateDocument.IndexOf(",") + 1));

                byte[] FamilyPaper;
                FamilyPaper = Convert.FromBase64String(candidateDocument.FamilyPaper.Substring(candidateDocument.BirthCertificateDocument.IndexOf(",") + 1));

                byte[] AbsenceOfPrecedents;
                AbsenceOfPrecedents = Convert.FromBase64String(candidateDocument.AbsenceOfPrecedents.Substring(candidateDocument.BirthCertificateDocument.IndexOf(",") + 1));

                byte[] PaymentReceipt;
                PaymentReceipt = Convert.FromBase64String(candidateDocument.PaymentReceipt.Substring(candidateDocument.BirthCertificateDocument.IndexOf(",") + 1));

                string subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Attachments", candidate.CandidateId.ToString());

                string birthCertificateDocumentFileName = @"Section" + Guid.NewGuid() + ".pdf";
                List<string> filesName = new List<string> {
                    @"BirthCertificate" + Guid.NewGuid() + ".pdf",
                    @"Nid" + Guid.NewGuid() + ".pdf",
                    @"FamilyPaper" + Guid.NewGuid() + ".pdf",
                    @"AbsenceOfPrecedents" + Guid.NewGuid() + ".pdf",
                    @"PaymentReceipt" + Guid.NewGuid() + ".pdf",
                 };
                string birthCertificateDirectory = "/PDF/" + candidate.CandidateId.ToString() + "/" + birthCertificateDocumentFileName;

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
               
                var attachments = new CandidateAttachments();
                attachments.BirthDateCertificate = filesDirectories["BirthCertificate"];
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

         [HttpGet("CompleteRegistration/{NationalId}")]

        public IActionResult CompleteRegistration([FromRoute] string NationalId)
        {
            try
            {
                var selectedCandidate = db.Candidates.Where(x => x.Nid == NationalId )
                    .Join(
                    db.ConstituencyDetails,
                     candidate => candidate.CandidateId,
                     subconstituency => subconstituency.ConstituencyDetailId,
                     (candidate, subconstituency) => new
                      {
                         Nid = candidate.Nid,
                         FirstName = candidate.FirstName,
                         FatherName = candidate.FatherName,
                         SurName = candidate.SurName,
                         subconstituencyName = subconstituency.ArabicName,
                        
                     }
                    )
                    .Select( s => new { s.Nid, fullName = string.Format("{0} {1} {2}", s.FirstName, s.FatherName, s.SurName), s.subconstituencyName }).FirstOrDefault();
                

                return Ok(selectedCandidate);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

    }
}
