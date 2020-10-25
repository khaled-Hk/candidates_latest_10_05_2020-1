using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Vue.Controllers
{
    [ValidateAntiForgeryToken]
    [Produces("application/json")]
    [Route("Api/Admin/Branches")]
    public class BranchesController : Controller
    {
        private readonly CandidatesContext db;
        private Helper help;
        public BranchesController(CandidatesContext context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {
                IQueryable<Branches> BranchesQuery;
                BranchesQuery = from p in db.Branches
                                where p.Status != 9
                                select p;

                var BranchesCount = (from p in BranchesQuery
                                    select p).Count();

                var BrancheList = (from p in BranchesQuery
                                  orderby p.CreatedOn descending
                                  select new 
                                  {
                                      p.BrancheId,
                                      p.Status,
                                      p.ArabicName,
                                      p.EnglishName,
                                      p.Description
                                  }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Branches = BrancheList, count = BranchesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetAllBranches")]
        public IActionResult GetAllBranches()
        {
            try
            {
                return Ok(new { Branches = db.Branches.Where(x=>x.Status!=9).OrderByDescending(x=>x.CreatedOn).Select(p=> new {
                    p.BrancheId,
                    p.ArabicName,
                    p.EnglishName,
                }).ToList() });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddBranches([FromBody] Branches BrancheData)
        {
            try
            {
                if (BrancheData == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }
                if (string.IsNullOrWhiteSpace(BrancheData.ArabicName))
                {
                    return BadRequest("الرجاء إدخال اسم الفرع بالعربي");
                }

                if (string.IsNullOrWhiteSpace(BrancheData.EnglishName))
                {
                    return BadRequest("الرجاء إدخال اسم الفرع بالانجليزي");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                BrancheData.CreatedBy = userId;
                BrancheData.CreatedOn = DateTime.Now;
                BrancheData.Status = 1;
                db.Branches.Add(BrancheData);
                db.SaveChanges();

                return Ok("لـقد تم تسجيل الـفرع بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{BranchId}/Delete")]
        public IActionResult Delete(long BranchId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Branch = (from p in db.Branches
                              where p.BrancheId == BranchId
                              select p).SingleOrDefault();

                if (Branch == null)
                {
                    return NotFound("خــطأ : لا يمكن اجراء العملية المنطقة غير موجود");
                }

                Branch.Status = 9;
                Branch.ModifiedBy = userId;
                Branch.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تم مسح الـفرع بنـجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}