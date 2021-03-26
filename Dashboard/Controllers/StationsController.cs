using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using static Services.Helper;

namespace Dashboard.Controllers
{
    [ValidateAntiForgeryToken]
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
       
        [HttpGet("Get")]
        public IActionResult GetStationsBasedOn([FromQuery]int pageNo, [FromQuery] int pageSize, [FromQuery] long? centerId)
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

                if (centerId == null)
                    return BadRequest( "الرجاء إختيار المركز");

                IQueryable<Stations> StationsQuery;
                StationsQuery = from p in db.Stations
                               where p.ProfileId == UP.ProfileId && p.Status != 9 && p.CenterId == centerId
                               select p;


                var StationCount = (from p in StationsQuery
                                   select p).Count();

                var StationList = (from p in StationsQuery
                                  join c in db.Centers on p.CenterId equals c.CenterId
                                   where c.ProfileId == UP.ProfileId && c.Status != 9
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

        [HttpPost("Add")]
        public IActionResult CreateStations([FromBody]Stations stations)
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

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (stations.CenterId == null)
                    return BadRequest( "الرجاء إختيار المركز" );

                if (string.IsNullOrEmpty(stations.ArabicName) || string.IsNullOrWhiteSpace(stations.ArabicName))
                {
                    return BadRequest( "الرجاء إدخال اسم المحطة بالعربي");
                }

                if (string.IsNullOrEmpty(stations.EnglishName) || string.IsNullOrWhiteSpace(stations.EnglishName))
                {
                    return BadRequest( "الرجاء إدخال اسم المحطة بالانجليزي" );
                }

                var newStation = new Stations
                {
                    ArabicName = stations.ArabicName,
                    EnglishName = stations.EnglishName,
                    Description = stations.Description,
                    CenterId = stations.CenterId,
                    ProfileId = UP.ProfileId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Status = 1
                };

                db.Stations.Add(newStation);
                db.SaveChanges();

                return Ok( string.Format("تم إضافة المحطة {0} بنجاح", newStation.ArabicName));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Delete/{StationId}")]
        public IActionResult DeleteConstituency([FromRoute] long? StationId)
        {
            try
            {

                if (StationId == null)
                {
                    return BadRequest( "الرجاء إختيار المحطة" );
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

                var station = db.Stations.Where(x => x.StationId == StationId).FirstOrDefault();

                if (station == null)
                {
                    return BadRequest( "المحطة التي تم إختيارها غير متوفرة" );
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


                return Ok( string.Format("تم حذف المحطة {0} بنجاح", station.ArabicName));
            }
            catch
            {
                return StatusCode(500,  "حدث خطاء، حاول مجدداً");
            }
        }

        [HttpPost("Edit")]
        public IActionResult UpdateStation([FromBody] Stations stations)
        {
            try
            {
                if (stations == null)
                {
                    return BadRequest( "حدث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                if (stations.StationId == null)
                {
                    return BadRequest( "الرجاء إختيار المحطة" );
                }


                if (string.IsNullOrEmpty(stations.ArabicName) || string.IsNullOrWhiteSpace(stations.ArabicName))
                {
                    return BadRequest( "الرجاء إدخال اسم المحطة بالعربي" );
                }

                if (string.IsNullOrEmpty(stations.EnglishName) || string.IsNullOrWhiteSpace(stations.EnglishName))
                {
                    return BadRequest( "الرجاء إدخال اسم المحطة بالانجليزي" );
                }


                var selectedStation = db.Stations.Where(x => x.StationId == stations.StationId && x.Status != 9).FirstOrDefault();

                if (selectedStation == null)
                {
                    return BadRequest( "المحطة التي تم إختيارها غير متوفرة" );
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                selectedStation.ArabicName = stations.ArabicName;
                selectedStation.EnglishName = stations.EnglishName;
                selectedStation.Description = stations.Description;
                selectedStation.ModifiedBy = userId;
                selectedStation.ModifiedOn = DateTime.Now;

                db.Stations.Update(selectedStation);
                db.SaveChanges();


                return Ok(string.Format("تم تحديث المحطة {0} بنجاح", selectedStation.ArabicName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Get/{stationId}")]
        public IActionResult GetStationBasedOn([FromRoute] long? stationId)
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

                if (stationId == null)
                {
                    return BadRequest("الرجاء إختيار المحطة");
                }
                var selectedStation = db.Stations.Where(x => x.ProfileId == UP.ProfileId && x.StationId == stationId && x.Status == 1).Select(obj => new { obj.ArabicName, obj.EnglishName, obj.Description }).FirstOrDefault();

               
                return Ok(selectedStation);
            }
            catch (Exception ex)
            {
                return StatusCode(500,  ex.Message);
            }


        }
    }
}
