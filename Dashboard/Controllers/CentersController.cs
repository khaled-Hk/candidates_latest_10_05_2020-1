using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentersController : ControllerBase
    {
        private readonly CandidatesContext db;

        public CentersController(CandidatesContext context)
        {
            db = context;
        }

        // GET: api/Centers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Centers>>> GetCenters()
        {
            return await db.Centers.ToListAsync();
        }

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
        public IActionResult CreateConstituency([FromBody] Centers center)
        {
            try
            {
                if (center == null)
                {
                    return BadRequest(new { message = "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال" });
                }
                //if (center. == null)
                //{
                //    return BadRequest(new { message = "الرجاء إختيار المنطقة" });
                //}

                if (string.IsNullOrEmpty(center.ArabicName) || string.IsNullOrWhiteSpace(center.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالعربي" });
                }

                if (string.IsNullOrEmpty(center.EnglishName) || string.IsNullOrWhiteSpace(center.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المنطقة بالانجليزي" });
                }

                var newCenter = new Centers
                {
                    ArabicName = center.ArabicName,
                    EnglishName = center.EnglishName,
                    Description = center.Description,
                    OfficeId = center.OfficeId,
                    CreatedBy = center.CreatedBy,
                    CreatedOn = DateTime.Now,
                    Status = 1

                };

                db.Centers.Add(newCenter);
                db.SaveChanges();


                return Ok(new { newCenter.CenterId, message = string.Format("تم إضافة المركز  {0} بنجاح", newCenter.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }
    }
}
