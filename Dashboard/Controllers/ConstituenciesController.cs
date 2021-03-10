using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using static Services.Helper;

namespace Vue.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class ConstituenciesController : ControllerBase
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
                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }
                IQueryable<Constituencies> ConstituenciesQuery;
                ConstituenciesQuery = from p in db.Constituencies
                                      where p.Status != 9 && p.ProfileId == UP.ProfileId
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
                                            Region=p.Region.ArabicName
                                        }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                
                return Ok(new { ResponseCode = 0, ResponseMsg = new { Constituencies = ConstituencyList, count = ConstituenciesCount } });
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

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء الـتأكد من أنك قمت بتسجيل الدخول" });
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, new { message = "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي" });
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

                var newConstituency = new Constituencies
                {
                    ArabicName = constituency.ArabicName,
                    EnglishName = constituency.EnglishName,
                    RegionId = constituency.RegionId,
                    Description = constituency.Description,
                    OfficeId = constituency.OfficeId,
                    CreatedBy = UP.UserId,
                    CreatedOn = DateTime.Now,
                    Status = 1,
                    ProfileId= UP.ProfileId
                };

                db.Constituencies.Add(newConstituency);
                db.SaveChanges();
                return Ok(new { ResponseCode = 9, ResponseMsg = new { constituencyId = newConstituency.ConstituencyId, message = string.Format("تم إضافة الدائر الرئيسية {0} بنجاح", newConstituency.ArabicName) } });
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
                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }
                var selectConstituencies = db.Constituencies.Where(x => x.Status == 1 && x.ProfileId==UP.ProfileId).Select(obj => new { obj.ConstituencyId, obj.ArabicName, obj.EnglishName, obj.RegionId, obj.OfficeId, obj.CreatedOn}).ToList();
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

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }

                var selectConstituencies = db.Constituencies.Where(x => x.Status == 1 && x.RegionId == RegionId && x.ProfileId == UP.ProfileId ).Select(obj => new { value = obj.ConstituencyId, label = obj.ArabicName }).ToList();
                return Ok(new { Constituencies = selectConstituencies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetConstituenciesDetalsChairs/{ConstituencyId}")]
        public IActionResult GetConstituenciesDetalsChairs([FromRoute] long? ConstituencyId)
        {
            try
            {
                if (ConstituencyId == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الرئيسية");
                }

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }


                var ConstituencyDetails = db.ConstituencyDetails.Where(x => x.Status != 9
                    && x.ConstituencyId == ConstituencyId && x.ProfileId ==UP.ProfileId)
                    .Select(obj => new{
                        value = obj.ConstituencyId,
                        label = obj.ArabicName
                    }).ToList();

                var ChairsDetails = db.Chairs.Where(x => x.Status != 9
                    && x.ConstituencyId == ConstituencyId)
                    .Select(obj => new {
                        obj.ChairId,
                        obj.GeneralChairRemaining,
                        obj.PrivateChairRemaining,
                        obj.RelativeChairRemaining,
                    }).FirstOrDefault();



                return Ok(new { ConstituencyDetails = ConstituencyDetails, ChairsDetails= ChairsDetails });
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

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }

                var selectConstituency = db.Constituencies.Where(x => x.ConstituencyId == constituencyId && x.Status == 1 && x.ProfileId== UP.ProfileId).Select(obj => new { RegionId = obj.RegionId, ArabicName = obj.ArabicName, EnglishName = obj.EnglishName }).FirstOrDefault();
                return Ok(new { Constituency = selectConstituency });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetAConstituency/{regionId}")]
        public IActionResult GetAConstituencyBasedOn([FromRoute] long? regionId)
        {
            try
            {
                if (regionId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة");
                }

                UserProfile UP = this.help.GetProfileId(HttpContext, db);
                if (UP.UserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (UP.ProfileId <= 0)
                {
                    return StatusCode(401, "الرجاء تفعيل ضبط الملف الانتخابي التشغيلي");
                }

                var selectConstituency = db.Constituencies.Where(x => x.RegionId == regionId && x.Status == 1 && x.ProfileId == UP.ProfileId).Select(obj => new { value = obj.ConstituencyId, label = obj.ArabicName }).ToList();
                return Ok(new { Constituency = selectConstituency });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("ConstituencyPagination")]
        public IActionResult ConstituencyPagination([FromQuery]int pageNo, [FromQuery] int pageSize)
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

                IQueryable<Constituencies> ConstituencyQuery;
                ConstituencyQuery = from p in db.Constituencies
                                where p.Status != 9 && p.ProfileId== UP.ProfileId
                                select p;


                var ConstituenciesCount = (from p in ConstituencyQuery
                                           select p).Count();

                var ConstituenciesList = (from p in ConstituencyQuery
                                          join r in db.Regions on p.RegionId equals r.RegionId
                                          orderby p.CreatedOn descending
                                  select new
                                  {
                                      p.ConstituencyId,
                                      p.ArabicName,
                                      p.EnglishName,
                                      regionName = r.ArabicName,
                                      p.RegionId,
                                      p.OfficeId,
                                      p.CreatedOn,
                                      p.Status


                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Constituencies = ConstituenciesList, count = ConstituenciesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        }
}