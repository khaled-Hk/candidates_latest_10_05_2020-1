using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Dashboard.Controllers
{
   [AllowAnonymous]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class ConstituencyDetailsController : ControllerBase
    {
        private readonly CandidatesContext db;

        public ConstituencyDetailsController(CandidatesContext context)
        {
            db = context;
        }

        // GET: api/ConstituencyDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConstituencyDetails>>> GetConstituencyDetails()
        {
            return await db.ConstituencyDetails.ToListAsync();
        }

        // GET: api/ConstituencyDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConstituencyDetails>> GetConstituencyDetails(long id)
        {
            var constituencyDetails = await db.ConstituencyDetails.FindAsync(id);

            if (constituencyDetails == null)
            {
                return NotFound();
            }

            return constituencyDetails;
        }

        // PUT: api/ConstituencyDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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

        // POST: api/ConstituencyDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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

        [HttpPost("CreateConstituencyDetails")]
        public IActionResult CreateConstituency([FromBody] ConstituencyDetails constituencyDetails)
        {
            try
            {
                if (constituencyDetails == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }

                if (constituencyDetails.RegionId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                }

                if(constituencyDetails.ConstituencyId == 0)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الرئيسية" });
                }

                if (string.IsNullOrEmpty(constituencyDetails.ArabicName) || string.IsNullOrWhiteSpace(constituencyDetails.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة الفرعية بالعربي" });
                }

                if (string.IsNullOrEmpty(constituencyDetails.EnglishName) || string.IsNullOrWhiteSpace(constituencyDetails.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة الفرعية بالانجليزي" });
                }

                var newConstituencyDetails = new ConstituencyDetails
                {
                    ArabicName = constituencyDetails.ArabicName,
                    EnglishName = constituencyDetails.EnglishName,
                    ConstituencyId = constituencyDetails.ConstituencyId,
                    RegionId = constituencyDetails.RegionId,
                    Description = constituencyDetails.Description,
                    CreatedOn = DateTime.Now,
                    Status = 1

                };

                db.ConstituencyDetails.Add(newConstituencyDetails);
                db.SaveChanges();


                return Ok(new { constituencyId = newConstituencyDetails.ConstituencyId, message = string.Format("تم إضافة الدائر الفرعية {0} بنجاح", newConstituencyDetails.ArabicName) });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        private bool ConstituencyDetailsExists(long id)
        {
            return db.ConstituencyDetails.Any(e => e.ConstituencyDetailId == id);
        }

        [HttpDelete("DeleteConstituencyDetails/{ConstituencyDetailsId}")]
        public IActionResult DeleteConstituency([FromRoute] long? ConstituencyDetailsId)
        {
            try
            {

                if (ConstituencyDetailsId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                }

                var constituencyDetail = db.ConstituencyDetails.Where(x => x.ConstituencyDetailId == ConstituencyDetailsId).FirstOrDefault();

                if (constituencyDetail == null)
                {
                    return BadRequest(new { message = "المنطفة الفرعية التي تم إختيارها غير متوفرة" });
                }

                constituencyDetail.Status = 9;
                constituencyDetail.ModifiedOn = DateTime.Now ;

                db.ConstituencyDetails.Update(constituencyDetail);
                db.SaveChanges();


                return Ok(new { constituencyDetailId = constituencyDetail.ConstituencyDetailId, message = string.Format("تم حذف الدائر الرئيسية {0} بنجاح", constituencyDetail.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPut("UpdateConstituencyDetails")]
        public IActionResult UpdateConstituency([FromBody] ConstituencyDetails constituencyDetails)
        {
            try
            {
                if (constituencyDetails == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }

                if(constituencyDetails.ConstituencyDetailId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الفرعية" });
                }

                if (constituencyDetails.RegionId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                }

                if (string.IsNullOrEmpty(constituencyDetails.ArabicName) || string.IsNullOrWhiteSpace(constituencyDetails.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالعربي" });
                }

                if (string.IsNullOrEmpty(constituencyDetails.EnglishName) || string.IsNullOrWhiteSpace(constituencyDetails.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالانجليزي" });
                }

                if (constituencyDetails.ConstituencyId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الرئيسية" });
                }

                var selectedConstituencyDetails = db.ConstituencyDetails.Where(x => x.ConstituencyDetailId == constituencyDetails.ConstituencyDetailId).FirstOrDefault();

                if (selectedConstituencyDetails == null)
                {
                    return BadRequest(new { message = "المنطفة التي تم إختيارها غير متوفرة" });
                }
                selectedConstituencyDetails.ArabicName = constituencyDetails.ArabicName;
                selectedConstituencyDetails.EnglishName = constituencyDetails.EnglishName;
                selectedConstituencyDetails.RegionId = constituencyDetails.RegionId;
                selectedConstituencyDetails.Description = constituencyDetails.Description;
                selectedConstituencyDetails.ConstituencyId = constituencyDetails.ConstituencyId;
                selectedConstituencyDetails.ModifiedBy = constituencyDetails.ModifiedBy;
                selectedConstituencyDetails.ModifiedOn = DateTime.Now;

                db.ConstituencyDetails.Update(selectedConstituencyDetails);
                db.SaveChanges();


                return Ok(new { ConstituencyDetailId = selectedConstituencyDetails.ConstituencyDetailId, message = string.Format("تم تحديث الدائر الفرعية {0} بنجاح", selectedConstituencyDetails.ArabicName) });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetAllConstituencyDetails")]
        public IActionResult GetAllConstituencyDetails()
        {
            try
            {
                var selectConstituencyDetails = db.ConstituencyDetails.Where(x => x.Status == 1).Select(s => new {s.ConstituencyDetailId, s.ArabicName, s.EnglishName, s.ConstituencyId, s.CreatedOn}).ToList();
                var ConstituencyDetails = (from cd in selectConstituencyDetails join c in db.Constituencies on cd.ConstituencyId equals c.ConstituencyId select new { cd.ConstituencyDetailId, cd.ArabicName, cd.EnglishName, constituencyName = c.ArabicName , cd.CreatedOn }).ToList();
                return Ok(new { ConstituencyDetails });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetConstituencyDetail/{constituencyDetailId}")]
        public IActionResult GetConstituencyBasedOn([FromRoute] long? constituencyDetailId)
        {
            try
            {
                if (constituencyDetailId == null)
                {
                    return BadRequest("الرجاء إختيار المنطقة الفرعية");
                }
                var selectConstituencyDetail = db.ConstituencyDetails.Where(x => x.ConstituencyDetailId == constituencyDetailId && x.Status == 1).Select(obj => new { obj.ConstituencyId,RegionId = obj.RegionId, ArabicName = obj.ArabicName, EnglishName = obj.EnglishName }).FirstOrDefault();

                if (selectConstituencyDetail == null)
                    return BadRequest(new { message = "لا يوجد بيانات بالدائرة الفرعية التي تم إختيارها"});
                return Ok(new { ConstituencyDetail = selectConstituencyDetail });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }


        }

        [HttpGet("ConstituencyDetailsPagination")]
        public IActionResult ConstituencyDetailsPagination([FromQuery]int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                IQueryable<ConstituencyDetails> ConstituencyDetailsQuery;
                ConstituencyDetailsQuery = from p in db.ConstituencyDetails
                                           where p.Status != 9
                                    select p;


                var ConstituencyDetailsCount = (from p in ConstituencyDetailsQuery
                                           select p).Count();

                var ConstituencyDetailsList = (from p in ConstituencyDetailsQuery
                                               join mc in db.Constituencies on p.ConstituencyId equals mc.ConstituencyId
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

        [HttpGet("GetConstituencieyDetails")]
        public IActionResult GetConstituenciesBasedOn()
        {
            try
            {
                
                var selectConstituencyDetails = db.ConstituencyDetails.Where(x => x.Status == 1).Select(obj => new { value = obj.ConstituencyDetailId, label = obj.ArabicName }).ToList();
                return Ok(new { ConstituencyDetails = selectConstituencyDetails });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }
    }
}
