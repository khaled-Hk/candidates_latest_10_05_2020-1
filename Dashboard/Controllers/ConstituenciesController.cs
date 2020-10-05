using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Vue.Controllers
{
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

    }
}