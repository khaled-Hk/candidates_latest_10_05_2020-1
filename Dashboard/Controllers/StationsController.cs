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
    public class StationsController : ControllerBase
    {
        private readonly CandidatesContext db;
        private Helper help;

        public StationsController(CandidatesContext context)
        {
            db = context;
            help = new Helper();
        }

        // GET: api/Stations
       
        [HttpGet("GetStations")]
        public IActionResult GetStationsBasedOn([FromQuery]int pageNo, [FromQuery] int pageSize, [FromQuery] long? centerId)
        {
            try
            {
                if(centerId == null)
                    return BadRequest(new { message = "الرجاء إختيار المركز"});
                IQueryable<Stations> StationsQuery;
                StationsQuery = from p in db.Stations
                               where p.Status != 9 && p.CenterId == centerId
                               select p;


                var StationCount = (from p in StationsQuery
                                   select p).Count();

                var StationList = (from p in StationsQuery
                                  join c in db.Centers on p.CenterId equals c.CenterId
                                  
                                  orderby p.CreatedOn descending
                                  select new
                                  {
                                      p.StationId,
                                      p.ArabicName,
                                      p.EnglishName,
                                      centerName = c.ArabicName,
                                      p.CreatedOn,
                                      p.Status


                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Stations = StationList, count = StationCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("CreateStations")]
        public IActionResult CreateStations([FromBody]Stations stations)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (stations.CenterId == null)
                    return BadRequest(new { message = "الرجاء إختيار المركز" });

                if (string.IsNullOrEmpty(stations.ArabicName) || string.IsNullOrWhiteSpace(stations.ArabicName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المحطة بالعربي" });
                }

                if (string.IsNullOrEmpty(stations.EnglishName) || string.IsNullOrWhiteSpace(stations.EnglishName))
                {
                    return BadRequest(new { message = "الرجاء إدخال اسم المحطة بالانجليزي" });
                }

                var newStation = new Stations
                {
                    ArabicName = stations.ArabicName,
                    EnglishName = stations.EnglishName,
                    Description = stations.Description,
                    CenterId = stations.CenterId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Status = 1

                };

                db.Stations.Add(newStation);
                db.SaveChanges();


                return Ok(new { StationId = stations.StationId, message = string.Format("تم إضافة المحطة {0} بنجاح", newStation.ArabicName) });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("DeleteStation/{StationId}")]
        public IActionResult DeleteConstituency([FromRoute] long? StationId)
        {
            try
            {

                if (StationId == null)
                {
                    return BadRequest(new { message = "الرجاء إختيار المحطة" });
                }

                var station = db.Stations.Where(x => x.StationId == StationId).FirstOrDefault();

                if (station == null)
                {
                    return BadRequest(new { message = "المحطة التي تم إختيارها غير متوفرة" });
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                station.Status = 9;
                station.ModifiedOn = DateTime.Now;
                station.ModifiedBy = userId;
                db.Stations.Update(station);
                db.SaveChanges();


                return Ok(new { stationId = station.StationId, message = string.Format("تم حذف المحطة {0} بنجاح", station.ArabicName) });
            }
            catch
            {
                return StatusCode(500, new { message = "حدث خطاء، حاول مجدداً" });
            }
        }
    }
}
