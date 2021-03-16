using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Services.Helper;

namespace Dashboard.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class ConstituencyDetailsController : ControllerBase
    {
        private readonly CandidatesContext db;
        private Helper help;

        public ConstituencyDetailsController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        // GET: api/ConstituencyDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConstituencyDetails>> GetConstituencyDetails(long id)
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

            var constituencyDetails = await db.ConstituencyDetails.FindAsync(id);

            if (constituencyDetails == null)
            {
                return NotFound();
            }

            return constituencyDetails;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstituencyDetails(long id, ConstituencyDetails constituencyDetails)
        {
            if (id != constituencyDetails.ConstituencyDetailId)
            {
                return BadRequest();
            }

            db.Entry(constituencyDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConstituencyDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ConstituencyDetails>> PostConstituencyDetails(ConstituencyDetails constituencyDetails)
        {
            db.ConstituencyDetails.Add(constituencyDetails);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetConstituencyDetails", new { id = constituencyDetails.ConstituencyDetailId }, constituencyDetails);
        }

        // DELETE: api/ConstituencyDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ConstituencyDetails>> DeleteConstituencyDetails(long id)
        {
            var constituencyDetails = await db.ConstituencyDetails.FindAsync(id);
            if (constituencyDetails == null)
            {
                return NotFound();
            }

            db.ConstituencyDetails.Remove(constituencyDetails);
            await db.SaveChangesAsync();

            return constituencyDetails;
        }

        [HttpPost]
        public IActionResult CreateConstituency([FromBody] ConstituencyDetails constituencyDetails)
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

                if (constituencyDetails == null)
                {
                    return BadRequest("حدث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                if (constituencyDetails.RegionId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة");
                }

                if (constituencyDetails.ConstituencyId == 0)
                {
                    return BadRequest("الرجاء إختيار المنطقة الرئيسية");
                }

                if (string.IsNullOrEmpty(constituencyDetails.ArabicName) || string.IsNullOrWhiteSpace(constituencyDetails.ArabicName))
                {
                    return BadRequest("الرجاء إدخال اسم المنطقة الفرعية بالعربي");
                }

                if (string.IsNullOrEmpty(constituencyDetails.EnglishName) || string.IsNullOrWhiteSpace(constituencyDetails.EnglishName))
                {
                    return BadRequest("الرجاء إدخال اسم المنطقة الفرعية بالانجليزي");
                }

                var newConstituencyDetails = new ConstituencyDetails
                {
                    ArabicName = constituencyDetails.ArabicName,
                    EnglishName = constituencyDetails.EnglishName,
                    ConstituencyId = constituencyDetails.ConstituencyId,
                    RegionId = constituencyDetails.RegionId,
                    Description = constituencyDetails.Description,
                    ProfileId = UP.ProfileId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = UP.UserId,
                    Status = 1

                };

                db.ConstituencyDetails.Add(newConstituencyDetails);
                db.SaveChanges();


                return Ok(new { constituencyId = newConstituencyDetails.ConstituencyId, message = string.Format("تم إضافة الدائر الفرعية {0} بنجاح", newConstituencyDetails.ArabicName) });
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        private bool ConstituencyDetailsExists(long id)
        {
            return db.ConstituencyDetails.Any(e => e.ConstituencyDetailId == id);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteConstituency([FromRoute] long? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة");
                }
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var constituencyDetail = db.ConstituencyDetails.Where(x => x.ConstituencyDetailId == id).FirstOrDefault();

                if (constituencyDetail == null)
                {
                    return BadRequest("المنطفة الفرعية التي تم إختيارها غير متوفرة");
                }

                constituencyDetail.Status = 9;
                constituencyDetail.ModifiedOn = DateTime.Now;
                constituencyDetail.ModifiedBy = userId;

                db.ConstituencyDetails.Update(constituencyDetail);
                db.SaveChanges();


                return Ok(string.Format("تم حذف الدائر الرئيسية {0} بنجاح", constituencyDetail.ArabicName));
            }
            catch
            {
                return StatusCode(500, "حدث خطاء، حاول مجدداً");
            }
        }

        [HttpPut]
        public IActionResult UpdateConstituency([FromBody] ConstituencyDetails constituencyDetails)
        {
            try
            {
                if (constituencyDetails == null)
                {
                    return BadRequest("حدث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (constituencyDetails.ConstituencyDetailId == null || constituencyDetails.ConstituencyDetailId == 0)
                {
                    return BadRequest("الرجاء إختيار المنطقة الفرعية");
                }

                if (constituencyDetails.RegionId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة");
                }

                if (string.IsNullOrEmpty(constituencyDetails.ArabicName) || string.IsNullOrWhiteSpace(constituencyDetails.ArabicName))
                {
                    return BadRequest("الرجاء إدخال اسم المنطقة بالعربي");
                }

                if (string.IsNullOrEmpty(constituencyDetails.EnglishName) || string.IsNullOrWhiteSpace(constituencyDetails.EnglishName))
                {
                    return BadRequest("الرجاء إدخال اسم المنطقة بالانجليزي");
                }

                if (constituencyDetails.ConstituencyId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة الرئيسية");
                }

                var selectedConstituencyDetails = db.ConstituencyDetails.Where(x => x.ConstituencyDetailId == constituencyDetails.ConstituencyDetailId).FirstOrDefault();

                if (selectedConstituencyDetails == null)
                {
                    return BadRequest("المنطفة التي تم إختيارها غير متوفرة");
                }
                selectedConstituencyDetails.ArabicName = constituencyDetails.ArabicName;
                selectedConstituencyDetails.EnglishName = constituencyDetails.EnglishName;
                selectedConstituencyDetails.RegionId = constituencyDetails.RegionId;
                selectedConstituencyDetails.Description = constituencyDetails.Description;
                selectedConstituencyDetails.ConstituencyId = constituencyDetails.ConstituencyId;
                selectedConstituencyDetails.ModifiedBy = userId;
                selectedConstituencyDetails.ModifiedOn = DateTime.Now;

                db.ConstituencyDetails.Update(selectedConstituencyDetails);
                db.SaveChanges();


                return Ok(string.Format("تم تحديث الدائر الفرعية {0} بنجاح", selectedConstituencyDetails.ArabicName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("All")]
        public IActionResult GetAllConstituencyDetails()
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

                var selectConstituencyDetails = db.ConstituencyDetails.Where(x => x.Status == 1 && x.ProfileId == UP.ProfileId).Select(obj => new { value = obj.ConstituencyDetailId, label = obj.ArabicName }).ToList();
                return Ok(selectConstituencyDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult GetConstituencyDetailBasedOn([FromRoute] long? id)
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

                if (id == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الفرعية");
                }
                var selectConstituencyDetail = db.ConstituencyDetails.Where(x =>  x.ProfileId == UP.ProfileId && x.Status == 1 &&  x.ConstituencyDetailId == id).Select(obj => new { obj.ConstituencyId, RegionId = obj.RegionId, ArabicName = obj.ArabicName, EnglishName = obj.EnglishName }).ToList();


                var chairsDetails = db.Chairs.Where(x => x.ConstituencyId == id).SingleOrDefault();

                //if (selectConstituencyDetail == null)
                //    return BadRequest("لا يوجد بيانات بالدائرة الفرعية التي تم إختيارها"});
                return Ok(new { ConstituencyDetail = selectConstituencyDetail, chairsDetails = chairsDetails });
            }
            catch (Exception ex)
            {
                return StatusCode(500,  ex.Message);
            }


        }

        [HttpGet("Get/Constituency/{constituencyId}")]
        public IActionResult GetConstituencyDetailsBasedOn([FromRoute] long? constituencyId)
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

                if (constituencyId == null)
                {
                    return BadRequest("الرجاء إختيار الدائرة الفرعية");
                }
                var selectedConstituencyDetails = db.ConstituencyDetails.Where(x => x.ConstituencyId == constituencyId && x.ProfileId == UP.ProfileId && x.Status == 1).Select(obj => new { value = obj.ConstituencyDetailId, label = obj.ArabicName }).ToList();

                if (selectedConstituencyDetails.Count == 0)
                    return BadRequest("لا يوجد بيانات بالدائرة الرئيسية التي تم إختيارها");
                return Ok( selectedConstituencyDetails );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpGet]
        public IActionResult ConstituencyDetailsPagination([FromQuery] int pageNo, [FromQuery] int pageSize)
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

                IQueryable<ConstituencyDetails> ConstituencyDetailsQuery;
                ConstituencyDetailsQuery = from p in db.ConstituencyDetails
                                           where p.ProfileId == UP.ProfileId && p.Status != 9
                                           select p;


                var ConstituencyDetailsCount = (from p in ConstituencyDetailsQuery
                                                select p).Count();

                var ConstituencyDetailsList = (from p in ConstituencyDetailsQuery
                                               join mc in db.Constituencies on p.ConstituencyId equals mc.ConstituencyId
                                               where mc.ProfileId == UP.ProfileId && mc.Status != 9
                                               orderby p.CreatedOn descending
                                               select new
                                               {
                                                   p.ConstituencyDetailId,
                                                   p.ConstituencyId,
                                                   p.ArabicName,
                                                   p.EnglishName,
                                                   constituencyName = mc.ArabicName,
                                                   p.RegionId,
                                                   p.CreatedOn,
                                                   p.Status


                                               }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { ConstituencyDetails = ConstituencyDetailsList, count = ConstituencyDetailsCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}
