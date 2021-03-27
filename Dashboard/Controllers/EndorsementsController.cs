using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using static Services.Helper;

namespace Dashboard.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("Api/Admin/[controller]")]
    [ApiController]
    public class EndorsementsController : ControllerBase
    {
        private readonly CandidatesContext db;
        private Helper help;

        public EndorsementsController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }


        [HttpGet]
        public IActionResult GetEndorsements([FromQuery] long? candidateId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                if (candidateId == null || candidateId == 0)
                {
                    return BadRequest("الرجاء قم بإختيار المرشح" );
                }

                var candidate = db.Candidates.Where(x => x.CandidateId == candidateId).Select(x => new { x.FirstName, x.FatherName, x.GrandFatherName, x.SurName}).SingleOrDefault();
                var EndorsementsCount = db.Endorsements.Where(x => x.CandidateId == candidateId).Count();
                var EndorsementsList = db.Endorsements.Where(x => x.CandidateId == candidateId)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new
                    {
                        x.CandidateId,
                        x.Nid,
                        x.CreatedOn,
                        
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { Endorsements = EndorsementsList, count = EndorsementsCount, candidateName = string.Format("{0} {1} {2} {3}", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName) });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult GetEndorsementsByNationalId([FromQuery] string nationalId)
        {
            try
            {
                if (string.IsNullOrEmpty(nationalId))
                {
                    return BadRequest("الرجاء قم بإدخال الرقم الوطني" );
                }
                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" );
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي" );
                }

                var candidate = db.Candidates.Where(x => x.Nid == nationalId && x.ProfileId == UP.ProfileId).Select(x => new { x.FirstName, x.FatherName, x.GrandFatherName, x.SurName, x.Levels, x.CandidateId}).SingleOrDefault();

                if (candidate==null)
                {
                    return NotFound(string.Format("المرشح صاحب الرقم الوطني {0} لم يسجل من قبل ", nationalId));
                }

                if (candidate.Levels < 3)
                {
                    return BadRequest(string.Format("المرشح صاحب الرقم الوطني {0} لم يكمل عملية تسجيل",nationalId));
                }

                if (candidate.Levels == 3)
                {
                    return BadRequest( string.Format("المرشح {0} {1} {2} {3} لم يكمل عملية تسجيل", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName));
                }
           


                return Ok(new {candidateId = candidate.CandidateId, candidateName = string.Format("{0} {1} {2} {3}", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName) });
            }
            catch (Exception e)
            {
                return StatusCode(500,  e.Message );
            }
        }

        public class Endorsement
        {
           public string Nid { get; set; }
           public long? CandidateId { get; set; }
            public string CandidateNid { get; set; }
        }

        [HttpPost]
        public IActionResult NewEndorsement(Endorsement endorsement)
        {
            try
            {


              

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" });
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي" });
                }

                var profile = db.Profile.Where(x => x.ProfileId == UP.ProfileId).FirstOrDefault();

                if (endorsement.CandidateId == null || endorsement.CandidateId == 0)
                {
                    return BadRequest("الرجاء قم بإختيار المرشح");
                }

                if (string.IsNullOrWhiteSpace(endorsement.Nid.Trim()) || string.IsNullOrEmpty(endorsement.Nid.Trim()))
                {
                    return BadRequest("الرجاءإدخال الرقم الوطني" );
                }

                if (endorsement.Nid.Length != 12)
                {
                    return BadRequest("يجب أن يكون عدد الرقم الوطني 12 الحرف" );
                }

                var endorsementCount = db.Endorsements.Where(x => x.Nid == endorsement.Nid).Count();

                if (endorsementCount > 0)
                {
                    return BadRequest("من غير المسموح تجسيل المزكي أكثر من مرة" );
                }

                var endorsementsCount = db.Endorsements.Where(x => x.CandidateId == endorsement.CandidateId).Count();
               
                if (endorsementsCount == profile.Endorsements)
                {
                    return BadRequest( "لقد حصلت على العدد المطلوب من المزكين ومن غير المسموح لك بإضافة مزكي جديد" );
                }
                else
                {
                    db.Endorsements.Add(new Endorsements
                    {
                        Nid = endorsement.Nid,
                        CandidateId = endorsement.CandidateId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = UP.UserId
                    });
                    db.SaveChanges();
                }



             


                return Ok( "تم تسجيل المزكي بنجاح" );
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message );
            }
        }

        [HttpPost("Add")]
        public IActionResult AddNewEndorsement(Endorsement endorsement)
        {
            try
            {


                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401,"الرجاء الـتأكد من أنك قمت بتسجيل الدخول" );
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي" );
                }

                var profile = db.Profile.Where(x => x.Status == 1 && x.ProfileId == UP.ProfileId).FirstOrDefault(); 

                var candidate = db.Candidates.Where(x => x.Nid == endorsement.CandidateNid && x.ProfileId == profile.ProfileId).FirstOrDefault();


                if (candidate.CandidateId == null || candidate.CandidateId == 0)
                {
                    return BadRequest( "الرجاء قم بإختيار المرشح" );
                }

                if (string.IsNullOrWhiteSpace(endorsement.Nid.Trim()) || string.IsNullOrEmpty(endorsement.Nid.Trim()))
                {
                    return BadRequest( "الرجاءإدخال الرقم الوطني" );
                }

                //if (numericRegex.IsMatch(endorsement.Nid.Trim()))
                //{
                //    return BadRequest(new { message = "يجب أن يحتوي الرقم الوطني على أرقام فقط" });
                //}

                if (endorsement.Nid.Length != 12)
                {
                    return BadRequest( "يجب أن يكون عدد الرقم الوطني 12 الحرف" );
                }

                var endorsementCount = db.Endorsements.Where(x => x.Nid == endorsement.Nid).FirstOrDefault();

                if(endorsementCount != null)
                {
                    return BadRequest( "من غير المسموح تجسيل المزكي أكثر من مرة" );
                }


                var registeredEndorsements = db.Endorsements.Where( x => x.CandidateId == candidate.CandidateId).Count();
                int endorsementsCount = registeredEndorsements;
                if (endorsementsCount == profile.Endorsements)
                {
                    return BadRequest("لقد حصلت على العدد المطلوب من المزكين ومن غير المسموح لك بإضافة مزكي جديد" );
                }

                db.Endorsements.Add(new Endorsements
                {
                    Nid = endorsement.Nid,
                    CandidateId = candidate.CandidateId,
                    ProfileId = UP.ProfileId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = UP.ProfileId
                });
             

                if (endorsementsCount == profile.Endorsements)
                {
                  
                    candidate.Levels = 6;
                    candidate.Status = 1;
                    db.Candidates.Update(candidate);
                    db.SaveChanges();
                    return Ok(new { message = "تم تسجيل المزكي بنجاح" , level = candidate.Levels });
                }
                db.SaveChanges();

                return Ok(new { message = "تم الحصول على العدد الكافي من المزكين",  level = candidate.Levels });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message );
            }
        }


        private bool EndorsementsExists(long id)
        {
            return db.Endorsements.Any(e => e.EndorsementId == id);
        }
    }
}
