using System;
using System.Collections.Generic;
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
                var candidateUser = db.CandidateUsers.Where(x => x.CandidateId == candidateId).Select(x => new { x.CandidateUserId}).SingleOrDefault();
                var candidate = db.Candidates.Where(x => x.CandidateId == candidateId).Select(x => new { x.FirstName, x.FatherName, x.GrandFatherName, x.SurName}).SingleOrDefault();
                var EndorsementsCount = db.Endorsements.Where(x => x.CandidateUserId == candidateUser.CandidateUserId).Count();
                var EndorsementsList = db.Endorsements.Where(x => x.CandidateUserId == candidateUser.CandidateUserId)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new
                    {
                        x.CandidateUserId,
                        x.Nid,
                        x.CreatedOn,
                        
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { Endorsements = EndorsementsList, count = EndorsementsList, candidateName = string.Format("{0} {1} {2} {3}", candidate.FirstName, candidate.FatherName, candidate.GrandFatherName, candidate.SurName) });
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

                var selectedEndorsement = db.Endorsements.Where(x => x.Nid == endorsement.Nid).SingleOrDefault();

                if(selectedEndorsement != null)
                {
                    return BadRequest(new { message = "من غير المسموح تجسيل المزكي أكثر من مرة" });
                }


                var candidateUser = db.CandidateUsers.Where(x => x.CandidateId == endorsement.CandidateId).Select(x => new { x.CandidateUserId }).SingleOrDefault();

                db.Endorsements.Add(new Endorsements
                {
                    Nid = endorsement.Nid,
                    CandidateUserId = candidateUser.CandidateUserId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId
                });
                db.SaveChanges();

                return Ok(new { message = "تم تسجيل المزكي بنجاح"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }


        private bool EndorsementsExists(long id)
        {
            return db.Endorsements.Any(e => e.EndorsementId == id);
        }
    }
}
