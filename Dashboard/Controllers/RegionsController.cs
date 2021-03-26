using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Dashboard.Controllers
{
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    [Route("Api/Admin/Regions")]
    public class RegionsController : Controller
    {
        private readonly CandidatesContext db;
        private Helper help;
        public RegionsController(CandidatesContext context)
        {
            this.db = context;
            help = new Helper();
        }

        
        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {
                IQueryable<Regions> RegisonsQuery;
                RegisonsQuery = from p in db.Regions
                                where p.Status != 9
                                select p;


                var RegionsCount = (from p in RegisonsQuery
                                   select p).Count();

                var RegionList = (from p in RegisonsQuery
                                  orderby p.CreatedOn descending
                                  select new
                                  {
                                    p.RegionId,
                                    p.Status,
                                    p.ArabicName,
                                    p.EnglishName,
                                    
                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Regions = RegionList, count = RegionsCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddRegion([FromBody] Regions RegionData)
        {
            try
            {
                if (RegionData == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }
                if (string.IsNullOrWhiteSpace(RegionData.ArabicName))
                {
                    return BadRequest("الرجاء إدخال اسم المنطقة بالعربي");
                }

                if (string.IsNullOrWhiteSpace(RegionData.EnglishName))
                {
                    return BadRequest("الرجاء إدخال اسم المنطقة بالانجليزي");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                RegionData.CreatedBy = userId;
                RegionData.CreatedOn = DateTime.Now;
                RegionData.Status = 1;
                db.Regions.Add(RegionData);
                db.SaveChanges();

                return Ok("لـقد تم تسجيل المنطقـة بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{RegionId}/Delete")]
        public IActionResult Delete(long RegionId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Region = (from p in db.Regions
                               where p.RegionId == RegionId
                               select p).SingleOrDefault();

                if (Region == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المنطقة غير موجود");
                }

                Region.Status = 9;
                Region.ModifiedBy = userId;
                Region.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تم مسح المنطـقة بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{RegionId}/Disable")]
        public IActionResult DisableRegion(long RegionId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Region = (from p in db.Regions
                              where p.RegionId == RegionId
                              select p).SingleOrDefault();

                if (Region == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المنطقة غير موجود");
                }

                Region.Status = 2;
                Region.ModifiedBy = userId;
                Region.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تم إلغاء تفعيل المنطـقة بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{RegionId}/Enable")]
        public IActionResult EnableRegion(long RegionId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Region = (from p in db.Regions
                              where p.RegionId == RegionId
                              select p).SingleOrDefault();

                if (Region == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المنطقة غير موجود");
                }

                Region.Status = 1;
                Region.ModifiedBy = userId;
                Region.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تم تفعيل المنطـقة بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetRegions")]
        public IActionResult GetRegions()
        {
            try
            {
                var regions = db.Regions.Where(x => x.Status != 9).Select(s => new { value = s.RegionId, label = s.ArabicName});
                return Ok(regions);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}