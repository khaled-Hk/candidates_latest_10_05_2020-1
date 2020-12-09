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


namespace Dashboard.Controllers
{
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
            public long? BranchId { get; set; }
            public long? ProfileId { get; set; }
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {

                var OfficeCount = db.Offices.Where(x => x.Status != 9).Count();

                var OfficesList = db.Offices.Where(x=>x.Status!=9)
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
                var offices = db.Offices.Select(s => new { label = s.ArabicName, value = s.OfficeId }).ToList();
                return Ok(new {offices});
            }catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/deleteOffice")]
        public IActionResult deleteOffice(long id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var office = db.Offices.Where(x => x.OfficeId == id).SingleOrDefault();

                if (office == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المكتب غير موجود");
                }

                office.Status = 9;
                office.ModifiedBy = userId;
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
                if (OfficeData == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var IsExist = db.Offices.Where(x => x.Status != 9 && x.ArabicName == OfficeData.ArabicName && x.BranchId == OfficeData.BranchId).SingleOrDefault();

                Offices offices = new Offices();
                offices.ArabicName = OfficeData.ArabicName;
                offices.EnglishName = OfficeData.EnglishName;
                offices.Description = OfficeData.Description;
                offices.BranchId = OfficeData.BranchId;
                offices.CreatedBy = userId;
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