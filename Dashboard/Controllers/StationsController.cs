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
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly CandidatesContext db;

        public StationsController(CandidatesContext context)
        {
            db = context;
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
    }
}
