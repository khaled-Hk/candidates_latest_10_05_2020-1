using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Services;


namespace Dashboard.Controllers
{
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    [Route("Api/Admin/Profile")]
    public class ProfileController : Controller 
    {

        private readonly CandidatesContext db;
        private Helper help;

        public ProfileController(CandidatesContext context )
        {
            this.db = context;
            help = new Helper();
        }

      
        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {
                IQueryable<Profile> ProfilesQuery;
                ProfilesQuery = from p in db.Profile
                             where p.Status != 9
                             select p;


                var ProfilCount = (from p in ProfilesQuery
                                  select p).Count();

                var ProfilList = (from p in ProfilesQuery
                                orderby p.CreatedOn descending
                                select new
                                {
                                    p.Name,
                                    p.Description,
                                    p.StartDate,
                                    p.EndDate,
                                    p.IsActivate,
                                    p.ProfileType,
                                    p.CreatedOn,
                                    p.Status,
                                    p.ProfileId
                                }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Profile = ProfilList, count = ProfilCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("GetActiveProfile")]
        public IActionResult GetActiveProfile()
        {
            try
            {
                IQueryable<Profile> ProfilesQuery;
                ProfilesQuery = from p in db.Profile
                                where p.IsActivate == 1
                                select p;

                var ProfilList = (from p in ProfilesQuery
                                  orderby p.CreatedOn descending
                                  select new
                                  {
                                      p.Name,
                                      p.Description,
                                      p.StartDate,
                                      p.EndDate,
                                      p.IsActivate,
                                      p.ProfileType,
                                      p.CreatedOn,
                                      p.Status,
                                      p.ProfileId
                                  }).SingleOrDefault();

                return Ok(new { Profile = ProfilList });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetRuningProfile")]
        public IActionResult GetRuningProfile()
        {
            try
            {
                IQueryable<Profile> ProfilesQuery;
                ProfilesQuery = from p in db.Profile
                                where p.Status == 1
                                select p;

                var ProfilList = (from p in ProfilesQuery
                                  orderby p.CreatedOn descending
                                  select new
                                  {
                                      p.Name,
                                      p.Description,
                                      p.StartDate,
                                      p.EndDate,
                                      p.IsActivate,
                                      p.ProfileType,
                                      p.CreatedOn,
                                      p.Status,
                                      p.ProfileId
                                  }).SingleOrDefault();

                return Ok(new { Profile = ProfilList });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("GetAllProfiles")]
        public IActionResult GetAllProfiles()
        {
            try
            {
                return Ok(new { Profile = db.Profile.Where(x => x.Status != 9).OrderByDescending(x => x.CreatedOn)
                    .Select(p => new
                    {
                        p.Name,
                        p.ProfileType,
                        p.ProfileId
                    }).ToList()});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("Add")]
        public IActionResult AddProfile([FromBody] Profile ProfileData)
        {
            try
            {
                if (ProfileData == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                ProfileData.CreatedBy = userId;
                ProfileData.CreatedOn = DateTime.Now;
                ProfileData.Status = 2;
                ProfileData.IsActivate = 0;
                db.Profile.Add(ProfileData);
                db.SaveChanges();

                return Ok("لـقد تم تسجيل ملف الانتخابات بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{ProfileId}/ActivateProfile")]
        public IActionResult ActivateProfile(long ProfileId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Profile = (from p in db.Profile
                               where p.ProfileId == ProfileId && p.IsActivate== 0
                               select p).SingleOrDefault();

                if (Profile == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية الملف غير موجود");
                }

                Profile.IsActivate = 1;
                Profile.ModifiedBy = userId;
                Profile.ModifiedOn = DateTime.Now;
                db.SaveChanges();

                var Profilelist = db.Profile.Where(x=>x.ProfileId != Profile.ProfileId).ToList();
                foreach(var p in Profilelist)
                {
                    p.IsActivate = 0;
                    db.Profile.Update(p);
                }
                db.SaveChanges();
                return Ok("تــم تفعيل الملف الإنتخابي بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{ProfileId}/DeActivateProfile")]
        public IActionResult DeActivateProfile(long ProfileId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Profile = (from p in db.Profile
                               where p.ProfileId == ProfileId && p.IsActivate == 1
                               select p).SingleOrDefault();

                if (Profile == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية الملف غير موجود");
                }

                Profile.IsActivate = 0;
                Profile.ModifiedBy = userId;
                Profile.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تــم إلغاء تفعيل الملف الإنتخابي بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{ProfileId}/PlayProfile")]
        public IActionResult PlayProfile(long ProfileId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Profile = (from p in db.Profile
                               where p.ProfileId == ProfileId && p.Status == 2
                               select p).SingleOrDefault();

                if (Profile == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية الملف غير موجود");
                }

                Profile.Status = 1;
                Profile.ModifiedBy = userId;
                Profile.ModifiedOn = DateTime.Now;
                db.SaveChanges();

                var Profilelist = db.Profile.Where(x => x.ProfileId != Profile.ProfileId).ToList();
                foreach (var p in Profilelist)
                {
                    p.Status = 2;
                    db.Profile.Update(p);
                }
                db.SaveChanges();
                return Ok("تم تشغيل الضبط الانتخابي المطلوب بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{ProfileId}/PauseProfile")]
        public IActionResult PauseProfile(long ProfileId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Profile = (from p in db.Profile
                               where p.ProfileId == ProfileId && p.Status == 1
                               select p).SingleOrDefault();

                if (Profile == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية الملف غير موجود");
                }

                Profile.Status = 2;
                Profile.ModifiedBy = userId;
                Profile.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تم إيقـاف الضبط الانتخابي المطلوب بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}