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
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    [Route("Api/Admin/Chairs")]
    public class ChairsController : Controller 
    {

        private readonly CandidatesContext db;
        private Helper help;

        public ChairsController(CandidatesContext context )
        {
            
            this.db = context;
            help = new Helper();
        }

        public class ChairsObj
        {
            public long constituencyId { get; set; }
            //public int generalChairs { get; set; }
            //public int privateChairs { get; set; }
            
            
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {

                var Count = db.Chairs.Where(x => x.Status != 9).Count();

                var Info = db.Chairs.Where(x=>x.Status!=9)
                    .OrderByDescending(x=> x.CreatedOn)
                    .Select(x=> new
                    {
                        x.ChairId,
                        x.GeneralChairs,
                        x.PrivateChairs,
                        x.GeneralChairRemaining,
                        x.PrivateChairRemaining,
                        Constituency=db.Constituencies.Where(y=> y.ConstituencyId==x.ConstituencyId).SingleOrDefault().ArabicName,
                        x.Status,
                        x.CreatedOn,
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { chairs = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost("{id}/delete")]
        public IActionResult delete(long id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Chair = db.Chairs.Where(x => x.ChairId == id).SingleOrDefault();

                if (Chair == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المقعد غير موجود");
                }

                Chair.Status = 9;
                Chair.ModifiedBy = userId;
                Chair.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تــم حذف المقعد بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] ChairsObj form)
        {
            try
            {
                if (form == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                Chairs charis = new Chairs();
                //charis.ConstituencyId = form.constituencyId;
                //charis.PrivateChairs = form.privateChairs;
                //charis.GeneralChairs = form.generalChairs;
                //charis.GeneralChairRemaining = form.generalChairs;
                //charis.PrivateChairRemaining = form.privateChairs;
                //charis.CreatedBy = userId;
                charis.CreatedOn = DateTime.Now;
                db.Chairs.Add(charis);
                db.SaveChanges();

                return Ok(" تم اضافة المقعد  بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



    }
}