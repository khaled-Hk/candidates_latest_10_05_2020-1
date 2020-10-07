using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Vue.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("Api/Admin/Constituencies")]
    public class ConstituenciesController : Controller
    {

        private readonly CandidatesContext db;
        private Helper help;
        public ConstituenciesController(CandidatesContext context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {
                IQueryable<Constituencies> ConstituenciesQuery;
                ConstituenciesQuery = from p in db.Constituencies
                                where p.Status != 9
                                select p;

                var ConstituenciesCount = (from p in ConstituenciesQuery
                                    select p).Count();

                var ConstituencyList = (from p in ConstituenciesQuery
                                  orderby p.CreatedOn descending
                                  select new
                                  {
                                      p.ConstituencyId,
                                      p.OfficeId,
                                      p.Status,
                                      p.ArabicName,
                                      p.EnglishName,

                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Constituencies = ConstituencyList, count = ConstituenciesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("CreateConstituency")]
        public IActionResult CreateConstituency([FromBody] Constituencies constituency)
        {
            try
            {
                if (constituency == null)
                {
                   return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }
                if(constituency.RegionId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                }

                if (string.IsNullOrEmpty(constituency.ArabicName) || string.IsNullOrWhiteSpace(constituency.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالعربي" });
                }

                if (string.IsNullOrEmpty(constituency.EnglishName) || string.IsNullOrWhiteSpace(constituency.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالانجليزي" });
                }

                var newConstituency = new Constituencies
                {
                    ArabicName = constituency.ArabicName,
                    EnglishName = constituency.EnglishName,
                    RegionId = constituency.RegionId,
                    Description = constituency.Description,
                    OfficeId = constituency.OfficeId,
                    CreatedBy = constituency.CreatedBy,
                    CreatedOn = DateTime.Now,
                    Status = 1

                };

                db.Constituencies.Add(newConstituency);
                db.SaveChanges();


                return Ok(new {constituencyId = newConstituency.ConstituencyId, message = string.Format("تم إضافة الدائر الرئيسية {0} بنجاح",newConstituency.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpDelete("DeleteConstituency/{ConstituencyId}")]
        public IActionResult DeleteConstituency([FromRoute] long? ConstituencyId)
        {
            try
            {
                
                if (ConstituencyId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                }

                var constituency = db.Constituencies.Where(x => x.ConstituencyId == ConstituencyId).FirstOrDefault();

                if(constituency == null)
                {
                    return BadRequest(new { message = "المنطفة الرئيسية التي تم إختيارها غير متوفرة" });
                }

                constituency.Status = 9;
                constituency.ModifiedOn = DateTime.Now;
                db.Constituencies.Update(constituency);
                db.SaveChanges();


                return Ok(new { constituencyId = ConstituencyId, message = string.Format("تم حذف الدائر الرئيسية {0} بنجاح", constituency.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPut("UpdateConstituency")]
        public IActionResult UpdateConstituency([FromBody] Constituencies constituency)
        {
            try
            {
                if (constituency == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }
                if (constituency.RegionId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                }

                if (string.IsNullOrEmpty(constituency.ArabicName) || string.IsNullOrWhiteSpace(constituency.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالعربي" });
                }

                if (string.IsNullOrEmpty(constituency.EnglishName) || string.IsNullOrWhiteSpace(constituency.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالانجليزي" });
                }

                if (constituency.ConstituencyId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الرئيسية" });
                }

                var selectedConstituency = db.Constituencies.Where(x => x.ConstituencyId == constituency.ConstituencyId).FirstOrDefault();

                if (selectedConstituency == null)
                {
                    return BadRequest(new { message = "المنطفة التي تم إختيارها غير متوفرة" });
                }
                selectedConstituency.ArabicName = constituency.ArabicName;
                selectedConstituency.EnglishName = constituency.EnglishName;
                selectedConstituency.RegionId = constituency.RegionId;
                selectedConstituency.Description = constituency.Description;
                selectedConstituency.OfficeId = constituency.OfficeId;
                selectedConstituency.ModifiedBy = constituency.ModifiedBy;
                selectedConstituency.ModifiedOn = DateTime.Now;
                   
                db.Constituencies.Update(selectedConstituency);
                db.SaveChanges();


                return Ok(new { constituencyId = selectedConstituency.ConstituencyId, message = string.Format("تم تحديث الدائر الرئيسية {0} بنجاح", selectedConstituency.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetAllConstituencies")]
        public IActionResult GetAllConstituencies()
        {
            try
            {
                var selectConstituencies = db.Constituencies.Where(x => x.Status == 1).Select(obj => new { obj.ConstituencyId, obj.ArabicName, obj.EnglishName, obj.RegionId, obj.OfficeId, obj.CreatedOn}).ToList();
                var constituencies = (from sc in selectConstituencies join r in db.Regions on sc.RegionId equals r.RegionId where r.Status == 1 select new { sc.ConstituencyId, sc.EnglishName, sc.ArabicName, sc.RegionId, regionName = r.ArabicName, sc.OfficeId, sc.CreatedOn }).ToList();
                return Ok(new { Constituencies = constituencies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetConstituenciesBasedOn/{RegionId}")]
        public IActionResult GetConstituenciesBasedOn([FromRoute] long? RegionId)
        {
            try
            {
                if(RegionId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة");
                }
                var selectConstituencies = db.Constituencies.Where(x => x.Status == 1 && x.RegionId == RegionId).Select(obj => new { value = obj.ConstituencyId, label = obj.ArabicName }).ToList();
                return Ok(new { Constituencies = selectConstituencies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetConstituency/{constituencyId}")]
        public IActionResult GetConstituencyBasedOn([FromRoute] long? constituencyId)
        {
            try
            {
                if (constituencyId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة");
                }
                var selectConstituency = db.Constituencies.Where(x => x.ConstituencyId == constituencyId && x.Status == 1 ).Select(obj => new { RegionId = obj.RegionId, ArabicName = obj.ArabicName, EnglishName = obj.EnglishName }).FirstOrDefault();
                return Ok(new { Constituency = selectConstituency });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }



    }
}