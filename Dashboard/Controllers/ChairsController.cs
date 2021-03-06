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
    public class ChairsController : ControllerBase
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
            public string generalChairs { get; set; }
            public string privateChairs { get; set; }
            public string RelativeChairs { get; set; }

            public long? ConstituencyDetailId { get; set; }
            public long? ChairId { get; set; }


        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
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

                var Count = db.Chairs.Where(x => x.Status != 9 && x.ProfileId == UP.ProfileId).Count();

                var Info = db.Chairs.Where(x=>x.Status!=9 && x.ProfileId == UP.ProfileId)
                    .OrderByDescending(x=> x.CreatedOn)
                    .Select(x=> new
                    {
                        x.ChairId,
                        x.GeneralChairs,
                        x.PrivateChairs,
                        x.RelativeChairs,
                        x.GeneralChairRemaining,
                        x.PrivateChairRemaining,
                        x.RelativeChairRemaining,
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
        
        [HttpPost("{id}/Delete")]
        public IActionResult Delete(long id)
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

                var Chair = db.Chairs.Where(x => x.ChairId == id && x.ProfileId == UP.ProfileId).SingleOrDefault();

                if (Chair == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المقعد غير موجود");
                }

                Chair.Status = 9;
                Chair.ModifiedBy = UP.UserId;
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

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" });
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي" });
                }

                Chairs charis = new Chairs();
                charis.ConstituencyId = form.constituencyId;
                charis.PrivateChairs = int.Parse(form.privateChairs);
                charis.GeneralChairs = int.Parse(form.generalChairs);
                charis.RelativeChairs = int.Parse(form.RelativeChairs);
                charis.GeneralChairRemaining = int.Parse(form.generalChairs);
                charis.PrivateChairRemaining = int.Parse(form.privateChairs);
                charis.RelativeChairRemaining = int.Parse(form.RelativeChairs);
                charis.CreatedBy = UP.UserId;
                charis.ProfileId = UP.ProfileId;
                charis.CreatedOn = DateTime.Now;
                db.Chairs.Add(charis);
                db.SaveChanges();

                return Ok(" تم اضافة المقعد  بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        public class ChairsDetailsObj
        {
            public long constituencyId { get; set; }
            public string generalChairs { get; set; }
            public string privateChairs { get; set; }
            public string RelativeChairs { get; set; }

            public long? ConstituencyDetailId { get; set; }
            public long? ChairId { get; set; }


        }

        [HttpPost("Add/Details")]
        public IActionResult AddChairsDetails([FromBody] ChairsDetailsObj form)
        {
            try
            {
                if (form == null || form.ConstituencyDetailId <= 0 || form.ChairId <= 0)
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");

                var userId = this.help.GetCurrentUser(HttpContext);

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" });
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي" });
                }

                var chair = db.Chairs.Where(x => x.ChairId == form.ChairId && x.Status != 9 && x.ProfileId == UP.ProfileId).SingleOrDefault();
                if(chair==null)
                    return StatusCode(401, "لم يتم العتور علي المقعد الرجاء اعادة المحاولة");

                if(chair.GeneralChairRemaining< int.Parse(form.generalChairs) ||
                    chair.PrivateChairRemaining < int.Parse(form.privateChairs) ||
                    chair.RelativeChairRemaining < int.Parse(form.RelativeChairs) )
                    return StatusCode(401, "لقد تخطيت عدد المقاعد المسموح به الرجاء التاكد من البيانات");

                ChairDetails chairDetails = new ChairDetails();
                ConstituencyDetailChairs constituencyDetailChairs = new ConstituencyDetailChairs();

                chairDetails.GeneralChairs= int.Parse(form.generalChairs);
                chairDetails.PrivateChairs= int.Parse(form.privateChairs);
                chairDetails.RelativeChairs = int.Parse(form.RelativeChairs);
                chairDetails.ChairId = form.ChairId;
                chairDetails.CreatedBy = userId;
                chairDetails.CreatedOn = DateTime.Now;
                //db.ChairDetails.Add(chairDetails);
                
                constituencyDetailChairs.ConstituencyDetailId = form.ConstituencyDetailId;
                constituencyDetailChairs.ChairDetail = chairDetails;
                db.ConstituencyDetailChairs.Add(constituencyDetailChairs);

                chair.GeneralChairRemaining -= int.Parse(form.generalChairs);
                chair.PrivateChairRemaining -= int.Parse(form.privateChairs);
                chair.RelativeChairRemaining -= int.Parse(form.RelativeChairs);
                db.SaveChanges();

                return Ok(" تم حجز المقاعد  بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Get/Details")]
        public IActionResult GetChairsDetails(int pageNo, int pageSize)
        {
            try
            {

                var Count = db.ConstituencyDetailChairs.Count();

                var Info = db.ConstituencyDetailChairs
                    .Include(x => x.ChairDetail).Include(x=>x.ConstituencyDetail)
                    .Select(x => new
                    {
                        x.ChairDetail.PrivateChairs,
                        x.ChairDetail.GeneralChairs,
                        x.ChairDetail.RelativeChairs,
                        x.ChairDetail.CreatedOn,
                        x.ConstituencyDetail.ArabicName,
                        x.ConstituencyDetailChairId,
                        Constituency = db.Constituencies.Where(y => y.ConstituencyId == x.ConstituencyDetail.ConstituencyId).SingleOrDefault().ArabicName,
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { chairs = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{id}/Details/Delete")]
        public IActionResult deleteChairsDetails(long id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var ConstituencyDetailChairs = db.ConstituencyDetailChairs.Where(x => x.ConstituencyDetailChairId == id).Include(x=>x.ChairDetail).SingleOrDefault();

                if (ConstituencyDetailChairs == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المقعد غير موجود");
                }

                var chair = db.Chairs.Where(x => x.ChairId == ConstituencyDetailChairs.ChairDetail.ChairId).SingleOrDefault();
                chair.PrivateChairRemaining += ConstituencyDetailChairs.ChairDetail.PrivateChairs;
                chair.GeneralChairRemaining += ConstituencyDetailChairs.ChairDetail.GeneralChairs;
                chair.RelativeChairRemaining += ConstituencyDetailChairs.ChairDetail.RelativeChairs;
                db.ConstituencyDetailChairs.Remove(ConstituencyDetailChairs);
                db.SaveChanges();
                return Ok("تمت عملية الحذف بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



    }
}