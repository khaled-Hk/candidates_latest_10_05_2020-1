using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using static Services.Helper;

namespace Dashboard.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {

        private readonly CandidatesContext db;
        private Helper help;

        public OfficesController(CandidatesContext context )
        {
            
            this.db = context;
            help = new Helper();
        }

        public class OfficeObj
        {
            public string ArabicName { get; set; }
            public string EnglishName { get; set; }
            public string Description { get; set; }
           // public long? BranchId { get; set; }
          //  public long? ProfileId { get; set; }
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
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

                var OfficeCount = db.Offices.Where(x => x.Status != 9).Count();

                var OfficesList = db.Offices.Where(x=>x.Status!=9 && x.ProfileId == UP.ProfileId)
                    .OrderByDescending(x=> x.CreatedOn)
                    .Select(x=> new
                    {
                        x.OfficeId,
                        x.ArabicName,
                        BranchName=db.Branches.Where(y=>y.BrancheId==x.BranchId).SingleOrDefault().ArabicName,
                        x.EnglishName,
                        x.CreatedOn,
                        x.Centers,
                        x.Constituencies,
                        x.Description,
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { office = OfficesList, count = OfficeCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetAllOffices")]
        public IActionResult GetOffices()
        {
            try
            {
                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <=0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <=0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }
                var offices = db.Offices.Where(x=>x.ProfileId== UP.ProfileId).Select(s => new { label = s.ArabicName, value = s.OfficeId }).ToList();
                return Ok(offices);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Delete")]
        public IActionResult deleteOffice(long id)
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

                var office = db.Offices.Where(x => x.OfficeId == id).SingleOrDefault();

                if (office == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المكتب غير موجود");
                }

                office.Status = 9;
                office.ModifiedBy = UP.UserId;
                office.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تــم حذف المكتب الإنتخابي بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddOffice([FromBody] OfficeObj OfficeData)
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


                Offices offices = new Offices();
                offices.ArabicName = OfficeData.ArabicName;
                offices.EnglishName = OfficeData.EnglishName;
                offices.Description = OfficeData.Description;
                offices.ProfileId = UP.ProfileId;
                offices.CreatedBy = UP.UserId;
                offices.Status = 1;
                offices.CreatedOn = DateTime.Now;
                db.Offices.Add(offices);
                db.SaveChanges();

                return Ok(" تم اضافة المكتب  بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        

    }
}