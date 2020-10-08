using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

namespace Dashboard.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class CentersController : ControllerBase
    {
        private readonly CandidatesContext db;
        private Helper help;

        public CentersController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        // GET: api/Centers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Centers>>> GetCenters()
        //{
        //    return await db.Centers.ToListAsync();
        //}

        // GET: api/Centers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Centers>> GetCenters(long id)
        {
            var centers = await db.Centers.FindAsync(id);

            if (centers == null)
            {
                return NotFound();
            }

            return centers;
        }

        // PUT: api/Centers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCenters(long id, Centers centers)
        {
            if (id != centers.CenterId)
            {
                return BadRequest();
            }

            db.Entry(centers).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CentersExists(id))
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

        // POST: api/Centers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Centers>> PostCenters(Centers centers)
        {
            db.Centers.Add(centers);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetCenters", new { id = centers.CenterId }, centers);
        }

        // DELETE: api/Centers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Centers>> DeleteCenters(long id)
        {
            var centers = await db.Centers.FindAsync(id);
            if (centers == null)
            {
                return NotFound();
            }

            db.Centers.Remove(centers);
            await db.SaveChangesAsync();

            return centers;
        }

        private bool CentersExists(long id)
        {
            return db.Centers.Any(e => e.CenterId == id);
        }

        [HttpPost("CreateCenter")]
        public IActionResult CreateCenter([FromBody] Centers center)
        {
            try
            {
                if (center == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }
                if (center.ConstituencDetailId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الفرعية" });
                }

                if (string.IsNullOrEmpty(center.ArabicName) || string.IsNullOrWhiteSpace(center.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالعربي" });
                }

                if (string.IsNullOrEmpty(center.EnglishName) || string.IsNullOrWhiteSpace(center.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالانجليزي" });
                }

                

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                

                var newCenter = new Centers
                {
                    ArabicName = center.ArabicName,
                    EnglishName = center.EnglishName,
                    ConstituencDetailId = center.ConstituencDetailId,
                    OfficeId = center.OfficeId,
                    Description = center.Description,
                    Longitude = center.Longitude,
                    Latitude = center.Latitude,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Status = 1

                };
                db.Centers.Add(newCenter);
                db.SaveChanges();
                if (center.Stations.Count > 0)
                {
                    Stations stations;
                    foreach (var station in center.Stations)
                    {
                        stations = new Stations
                        {
                            ArabicName = station.ArabicName,
                            EnglishName = station.EnglishName,
                            Description = station.Description,
                            CenterId = newCenter.CenterId,
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now,
                            Status = 1
                        };

                        db.Stations.Add(stations);
                    }

                }
                db.SaveChanges();


                return Ok(new { newCenter.CenterId, message = string.Format("تم إضافة المركز  {0} بنجاح", newCenter.ArabicName) });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("CenterPagination")]
        public IActionResult CenterPagination([FromQuery]int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                IQueryable<Centers> CentersQuery;
                CentersQuery = from p in db.Centers
                                           where p.Status != 9
                                           select p;


                var CenterCount = (from p in CentersQuery
                                                select p).Count();

                var CenterList = (from p in CentersQuery
                                  join sc in db.ConstituencyDetails on p.ConstituencDetailId equals sc.ConstituencyDetailId
                                  orderby p.CreatedOn descending
                                               select new
                                               {
                                                   p.CenterId,
                                                   ConstituencyDetailId = p.ConstituencDetailId,
                                                   p.ArabicName,
                                                   p.EnglishName,
                                                   constituencyDetailName = sc.ArabicName,
                                                   p.CreatedOn,
                                                   p.Status


                                               }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Centers = CenterList, count = CenterCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("DeleteCenter/{CenterId}")]
        public IActionResult DeleteConstituency([FromRoute] long? CenterId)
        {
            try
            {

                if (CenterId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المركز" });
                }

                var center = db.Centers.Where(x => x.CenterId == CenterId).FirstOrDefault();

                if (center == null)
                {
                    return BadRequest(new { message = "المركز التي تم إختياره غير متوفر" });
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                center.Status = 9;
                center.ModifiedOn = DateTime.Now;
                center.ModifiedBy = userId;
                db.Centers.Update(center);
                db.SaveChanges();


                return Ok(new { centerId = center.CenterId, message = string.Format("تم حذف مركز {0} بنجاح", center.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetCenter/{centerId}")]
        public IActionResult GetCenterBasedOn([FromRoute] long? centerId)
        {
            try
            {
                if (centerId == null)
                {
                    return BadRequest("الرجاء إختيار المركز");
                }
                var selectCenter = db.Centers.Where(x => x.CenterId == centerId && x.Status == 1).Select(obj => new { ConstituencyDetailId = obj.ConstituencDetailId, ArabicName = obj.ArabicName, EnglishName = obj.EnglishName, obj.Description, obj.Latitude, obj.Longitude }).FirstOrDefault();
                return Ok(new { Center = selectCenter });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpGet("GetCenters")]
        public IActionResult GetCenters()
        {
            try
            {
                
                var selectCenters = db.Centers.Where(x => x.Status == 1 ).Select(obj => new { value = obj.ConstituencDetailId, label = obj.ArabicName }).ToList();
                return Ok(new { Centers = selectCenters });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }

        [HttpPut("UpdateCenter")]
        public IActionResult UpdateCenter([FromBody] Centers center)
        {
            try
            {
                if (center == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }

                if (center.ConstituencDetailId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الفرعية" });
                }

                if (center.Latitude == null || center.Latitude.Equals("0.0"))
                {
                    return BadRequest(new { message = "الرجاء إدخال خط العرض" });
                }

                if (center.Longitude == null || center.Longitude.Equals("0.0"))
                {
                    return BadRequest(new { message = "الرجاء إدخال خط الطول" });
                }

                if (center.ConstituencDetailId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المنطقة الفرعية" });
                }


                if (string.IsNullOrEmpty(center.ArabicName) || string.IsNullOrWhiteSpace(center.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المركز بالعربي" });
                }

                if (string.IsNullOrEmpty(center.EnglishName) || string.IsNullOrWhiteSpace(center.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المركز بالانجليزي" });
                }

                if (center.CenterId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المركز" });
                }

                var selectedCenter = db.Centers.Where(x => x.CenterId == center.CenterId && x.Status != 9).FirstOrDefault();

                if (selectedCenter == null)
                {
                    return BadRequest(new { message = "المركز التي تم إختيارها غير متوفرة" });
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                selectedCenter.ArabicName = center.ArabicName;
                selectedCenter.EnglishName = center.EnglishName;
                selectedCenter.ConstituencDetailId = center.ConstituencDetailId;
                selectedCenter.OfficeId = center.OfficeId;
                selectedCenter.Latitude = center.Latitude;
                selectedCenter.Longitude = center.Longitude;
                selectedCenter.Description = center.Description;
                selectedCenter.ModifiedBy = userId;
                selectedCenter.ModifiedOn = DateTime.Now;

                db.Centers.Update(selectedCenter);
                db.SaveChanges();


                return Ok(new { CenterId = selectedCenter.CenterId, message = string.Format("تم تحديث المركز {0} بنجاح", selectedCenter.ArabicName) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex = ex.InnerException.Message, message = "حدث خطاء، حاول مجدداً" });
            }
        }
    }
}
