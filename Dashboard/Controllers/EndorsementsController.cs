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
                    return BadRequest(new { message = "الرجاء قم بإختيار المرشح" });
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
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("Get")]
        public IActionResult GetEndorsementsByNationalId([FromQuery] string nationalId)
        {
            try
            {
                if (string.IsNullOrEmpty(nationalId))
                {
                    return BadRequest(new { message = "الرجاء قم بإدخال الرقم الوطني" });
                }

                var candidate = db.Candidates.Where(x => x.Nid == nationalId).Select(x => new { x.FirstName, x.FatherName, x.GrandFatherName, x.SurName, x.Levels, x.CandidateId}).SingleOrDefault();

                if (candidate==null)
                {
                    return BadRequest(new { message = string.Format("المرشح صاحب الرقم الوطني {0} لم يسجل من قبل ", nationalId) });
                }

                if (candidate.Levels < 3)
                {
                    return BadRequest(new { message = string.Format("المرشح صاحب الرقم الوطني {0} لم يكمل عملية تسجيل",nationalId) });
                }

                if (candidate.Levels == 3)
                {
                    return BadRequest(new { message = string.Format("المرشح {0} {1} {2} {3} لم يكمل عملية تسجيل", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName) });
                }
           


                return Ok(new {candidateId = candidate.CandidateId, candidateName = string.Format("{0} {1} {2} {3}", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName) });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
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


                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }


                if (endorsement.CandidateId == null || endorsement.CandidateId == 0)
                {
                    return BadRequest(new { message = "الرجاء قم بإختيار المرشح" });
                }

                if (string.IsNullOrWhiteSpace(endorsement.Nid.Trim()) || string.IsNullOrEmpty(endorsement.Nid.Trim()))
                {
                    return BadRequest(new { message = "الرجاءإدخال الرقم الوطني" });
                }

                //if (numericRegex.IsMatch(endorsement.Nid.Trim()))
                //{
                //    return BadRequest(new { message = "يجب أن يحتوي الرقم الوطني على أرقام فقط" });
                //}

                if (endorsement.Nid.Length != 12)
                {
                    return BadRequest(new { message = "يجب أن يكون عدد الرقم الوطني 12 الحرف" });
                }

                var endorsementCount = db.Endorsements.Where(x => x.Nid == endorsement.Nid).Count();

                if (endorsementCount > 0)
                {
                    return BadRequest(new { message = "من غير المسموح تجسيل المزكي أكثر من مرة" });
                }

                if (endorsementCount == 1)
                {
                    return BadRequest(new { message = "من غير المسموح تجسيل المزكي أكثر من مرة" });
                }

                db.Endorsements.Add(new Endorsements
                {
                    Nid = endorsement.Nid,
                    CandidateId = endorsement.CandidateId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId
                });


                return Ok(new { message = "تم تسجيل المزكي بنجاح" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.InnerException.Message });
            }
        }

        [HttpPost("Add")]
        public IActionResult AddNewEndorsement(Endorsement endorsement)
        {
            try
            {
      

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var profile = db.Profile.Where(x => x.Status == 1).FirstOrDefault(); 

                var candidate = db.Candidates.Where(x => x.Nid == endorsement.CandidateNid && x.ProfileId == profile.ProfileId).FirstOrDefault();
                Debug.WriteLine(candidate.CandidateId + " // " + profile.ProfileId);

                if (candidate.CandidateId == null || candidate.CandidateId == 0)
                {
                    return BadRequest(new { message = "الرجاء قم بإختيار المرشح" });
                }

                if (string.IsNullOrWhiteSpace(endorsement.Nid.Trim()) || string.IsNullOrEmpty(endorsement.Nid.Trim()))
                {
                    return BadRequest(new { message = "الرجاءإدخال الرقم الوطني" });
                }

                //if (numericRegex.IsMatch(endorsement.Nid.Trim()))
                //{
                //    return BadRequest(new { message = "يجب أن يحتوي الرقم الوطني على أرقام فقط" });
                //}

                if (endorsement.Nid.Length != 12)
                {
                    return BadRequest(new { message = "يجب أن يكون عدد الرقم الوطني 12 الحرف" });
                }

                var endorsementCount = db.Endorsements.Where(x => x.Nid == endorsement.Nid).FirstOrDefault();

                if(endorsementCount != null)
                {
                    return BadRequest(new { message = "من غير المسموح تجسيل المزكي أكثر من مرة" });
                }


                var registeredEndorsements = db.Endorsements.Where( x => x.CandidateId == candidate.CandidateId).Count();
                int endorsementsCount = registeredEndorsements;
                if (endorsementsCount == 2)
                {
                    return BadRequest(new { message = "لقد حصلت على العدد المطلوب من المزكين ومن غير المسموح لك بإضافة مزكي جديد" });
                }

                db.Endorsements.Add(new Endorsements
                {
                    Nid = endorsement.Nid,
                    CandidateId = candidate.CandidateId,
                    ProfileId = profile.ProfileId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId
                });
             

                if (endorsementsCount == 1)
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
                return StatusCode(500, new { message = e.InnerException.Message });
            }
        }


        private bool EndorsementsExists(long id)
        {
            return db.Endorsements.Any(e => e.EndorsementId == id);
        }
    }
}
