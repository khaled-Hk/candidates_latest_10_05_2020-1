using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Dashboard.Controllers
{
    [Produces("application/json")]
    [Route("Security")]
    public class SecurityController : Controller
    {
        [TempData]
        public string ErrorMessage { get; set; }
        private Helper help;
        private readonly CandidatesContext db;
        private IConfiguration Configuration { get; }
        public SecurityController(CandidatesContext context, IConfiguration configuration)
        {
            this.db = context;
            help = new Helper();
            Configuration = configuration;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccountActivate(string confirm, string account)
        {
            try
            {

                confirm = confirm + "@cra.gov.ly";
                account = Security.DecryptBase64(account);

                if (!Security.VerifyHash(confirm, account, HashAlgorithms.SHA512))
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ViewData["ApiServer"] = Configuration.GetSection("Links")["ApiServer"];
                    return View();
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

       


        [AllowAnonymous]
        [HttpGet("Login")] 
        public async Task<IActionResult> Login(string returnUrl = null)
        {

            //bool isAuthenticated = User.Identity.IsAuthenticated;
            //if (isAuthenticated)
            //{
            //    return RedirectPermanent("/");
            //}
            //else
            //{
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return View();
            //  //  return RedirectPermanent("/Login");
            //}
        }


        [AllowAnonymous]
        [HttpGet("IsLogin")]
        public async Task<IActionResult> IsLogin(string returnUrl = null)
        {

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return Ok(true);
            }
            else
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok(false);
            }
        }


        public class user
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class userPassword
        {
            public int UserId { get; set; }
            public string NewPassword { get; set; }
            public string Password { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("loginUser")]
        public async Task<IActionResult> loginUser([FromBody] user loginUser)
        {
            try
            {
                if (loginUser == null)
                {
                    return NotFound("الرجاء ادخال البريد الالكتروني او اسم الدخول");
                }

                //if (!Validation.IsValidEmail(loginUser.Email))
                //{
                //    return BadRequest("Please enter correct email address");
                //}
                if (string.IsNullOrWhiteSpace(loginUser.Email))
                {
                    return BadRequest("الرجاء ادخال البريد الالكتروني او اسم الدخول");
                }

                if (string.IsNullOrWhiteSpace(loginUser.Password))
                {
                    return BadRequest("الرجاء ادخال كلمه المرور");
                }

                var cUser = (from p in db.Users
                             where (p.Email == loginUser.Email || p.LoginName == loginUser.Email) && p.State != 9
                             select p).SingleOrDefault();

                if (cUser == null)
                {
                    return NotFound("الرجاء التاكد من البريد الالكتروني وكلمة المرور");

                }

                if (cUser.UserType != 1 && cUser.UserType != 2 && cUser.UserType != 3)
                {
                    return BadRequest("ليس لديك صلاحيه للدخول علي النظام");
                }

                if (cUser.State == 0)
                {
                    return BadRequest("حسابك غير مفعل");
                }
                if (cUser.State == 2)
                {
                    if (cUser.LoginTryAttemptDate != null)
                    {
                        DateTime dt = (DateTime)cUser.LoginTryAttemptDate;
                        double minuts = 30;
                        dt = dt.AddMinutes(minuts);
                        if (dt >= DateTime.Now)
                        {
                            return BadRequest("لايمكنك الدخول للنظام: تم ايقافك");
                        }
                        else
                        {
                            cUser.State = 1;

                            db.SaveChanges();
                        }
                    }
                    else { return BadRequest("لايمكنك الدخول للنظام: تم ايقافك"); }
                }

                if (!Security.VerifyHash(loginUser.Password, cUser.Password, HashAlgorithms.SHA512))
                {

                    cUser.LoginTryAttempts++;
                    if (cUser.LoginTryAttempts >= 5 && cUser.State == 1)
                    {
                        cUser.LoginTryAttemptDate = DateTime.Now;
                        cUser.State = 2;
                    }
                    db.SaveChanges();
                    return NotFound("الرجاء التاكد من البريد الالكتروني وكلمة المرور");
                }
                //string hospital = "";
                //if (cUser.UserType == 5 && cUser.HospitalId != null && cUser.HospitalId>0)
                //{
                //    hospital = db.Hospital.Where(x => x.HospitalId == cUser.HospitalId).SingleOrDefault().Name;
                //}

                cUser.LoginTryAttempts = 0;
                cUser.LastLoginOn = DateTime.Now;
                db.SaveChanges();
                long branchId = -1;
                // int branchType = -1;
                string brancheName = "";

                if (cUser.UserType == 1)
                {

                    // branchType = (int)cUser.Office.OfficeType;

                    //     if (officeType==1)
                    //     {
                    //          issusId = db.Offices.AsEnumerable().Where(x => x.OfficeIndexId == officeId)
                    //.Select(r => (long?)r.OfficeId)
                    //.ToArray();

                    //          CivilId = db.Offices.AsEnumerable().Where(x => issusId.ToList().Contains(x.OfficeIndexId))
                    //     .Select(r => (long?)r.OldOfficeId)
                    //     .ToArray();
                    //     } else if(officeType == 2)
                    //     {
                    //          CivilId = db.Offices.AsEnumerable().Where(x => x.OfficeIndexId == officeId)
                    //    .Select(r => (long?)r.OldOfficeId).ToArray();

                    //     }
                    //     else {
                    //         CivilId = db.Offices.AsEnumerable().Where(x => x.OfficeId == officeId)
                    // .Select(r => (long?)r.OldOfficeId).ToArray();
                    //     }


                }
                var userInfo = new
                {
                    userId = cUser.Id,
                    fullName = cUser.Name,
                    userType = cUser.UserType,
                    branchId = branchId,
                    // officeType = officeType,
                    brancheName = brancheName,
                    LoginName = cUser.LoginName,
                    DateOfBirth = cUser.BirthDate,
                    Email = cUser.Email,
                    //cUser.Office.OfficeName,
                    Gender = cUser.Gender,
                    Status = cUser.State,
                    Phone = cUser.Phone
                };

                const string Issuer = "http://www.nid.ly";
                var claims = new List<Claim>();
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id", cUser.Id.ToString(), ClaimValueTypes.Integer64, Issuer));
                claims.Add(new Claim(ClaimTypes.Name, cUser.Name, ClaimValueTypes.String, Issuer));
                //claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/OfficeId", cUser.BranchId.ToString(), ClaimValueTypes.Integer64, Issuer));
                claims.Add(new Claim("userType", cUser.UserType.ToString(), ClaimValueTypes.Integer32, Issuer));
                var userIdentity = new ClaimsIdentity("thisisasecreteforauth");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(1),
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                return Ok(userInfo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "error while logout");
            }
        }


        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] userPassword loginUser)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (loginUser.Password != null)
                {
                    var User = db.Users.Where(x => x.Id == userId && x.State != 9).SingleOrDefault();
                    

                    if (Security.VerifyHash(loginUser.Password, User.Password, HashAlgorithms.SHA512))
                    {

                        User.Password = Security.ComputeHash(loginUser.NewPassword, HashAlgorithms.SHA512, null);
                        User.ModifiedBy = userId;
                        User.ModifiedOn = DateTime.Now;
                        db.SaveChanges();


                    }
                    else
                    {
                        return BadRequest("الرجاء التاكد من كلمة المرور");
                    }
                }

                else
                {
                    var User = db.Users.Where(x => x.Id == loginUser.UserId && x.State != 9).SingleOrDefault();
                    
                    if (User == null)
                    {
                        return BadRequest("خطأ بيانات المستخدم غير موجودة");
                    }
                    User.Password = Security.ComputeHash(loginUser.NewPassword, HashAlgorithms.SHA512, null);
                    User.ModifiedBy = userId;
                    User.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                }
                return Ok("تمت عمليه تعديل بنجاح");
            }
            catch (Exception)
            {
                return StatusCode(500, "error while logout");
            }

        }


        //[HttpGet]

        //public IActionResult GetUserImage(long userId)
        //{
        //    var userimage = (from p in db.Users
        //                     where p.UserId == userId
        //                     select p.Photo).SingleOrDefault();

        //    return File(userimage, "image/jpg");
        //}

        //public IActionResult Error()
        //{
        //    ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        //    return View();
        //}

        //public IActionResult Unsupported()
        //{
        //    ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        //    return View();
        //}

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }



        //[HttpPost("Security/ResetPassword/{email}")]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string email)
        //{
        //    try
        //    {

        //        if (!Validation.IsValidEmail(email))
        //        {
        //            return BadRequest("الرجاء ادخال البريد الالكتروني بطريقة الصحيحه");
        //        }

        //        var user = (from p in db.Users
        //                    where p.Email == email && p.Status != 9
        //                    select p).SingleOrDefault();

        //        if (user == null)
        //        {
        //            return NotFound("البريد الإلكتروني غير مسجل بالنظـام !");
        //        }

        //        if (user.Status == 0)
        //        {
        //            return BadRequest("تم إيقاف هذا المستخدم من النظام !");
        //        }


        //        MailMessage mail = new MailMessage();

        //        mail.From = new MailAddress("noreply@cra.gov.ly");

        //        mail.To.Add(email);

        //        string confirm = Security.ComputeHash(user.UserId.ToString() + "@cra.gov.ly", HashAlgorithms.SHA512, null);

        //        mail.Subject = "مصلحة الاحوال المدنية - إعادة تعيين كلمة المرور";

        //        mail.Body = GetResetPasswordHTML(user.FullName, "/security/AccountActivate?confirm=" + user.UserId.ToString() + "&account=" + Security.EncryptBase64(confirm));

        //        mail.IsBodyHtml = true;

        //        var smtp = new SmtpClient("webmail.cra.gov.ly")
        //        {
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential("noreply@cra.gov.ly", "Qwerty@!@#123"),
        //            Port = Int32.Parse(Configuration.GetSection("Links")["SMTPPORT"]),
        //            EnableSsl = Configuration.GetSection("Links")["SMTSSL"] == "1"
        //        };
        //        smtp.Send(mail);
        //        //Task.Factory.StartNew(() => smtp.Send(mail));
        //        return Ok("تم ارسال بريد التحقق بنجاح الرجاء فتح بريدك الإلكتروني");
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);

        //    }
        //}

        //public class UserActivation
        //{
        //    public string password { get; set; }
        //    public string cpassword { get; set; }
        //    public int confirm { get; set; }
        //    public string account { get; set; }
        //}

        //[AllowAnonymous]
        //[HttpPost("Security/ActivateUser")]
        //public IActionResult ActivateUser([FromBody] UserActivation userActivate)
        //{
        //    try
        //    {
        //        if (!Security.VerifyHash(userActivate.confirm.ToString() + "@cra.gov.ly", Security.DecryptBase64(userActivate.account), HashAlgorithms.SHA512))
        //        {
        //            return BadRequest("الرابط غير مفعل");
        //        }

        //        var user = (from u in db.Users
        //                    where u.UserId == userActivate.confirm
        //                    select u).SingleOrDefault();

        //        if (user == null)
        //        {
        //            return NotFound(" المستخدم غير موجود ");
        //        }


        //        if (String.IsNullOrEmpty(userActivate.password))
        //        {
        //            return BadRequest("يجب إدخال كلمة المرور");
        //        }
        //        else if (userActivate.password.Length <= 7)
        //        {
        //            return BadRequest("يجب ان تكون كلمة المرور اكبر من سبع خانات");
        //        }
        //        else if (String.IsNullOrEmpty(userActivate.cpassword))
        //        {
        //            return BadRequest("يجب إدخال تأكيد كلمة المرور ");
        //        }
        //        if (userActivate.password != userActivate.cpassword)
        //        {
        //            return BadRequest("خطأ في عملية المطابقة لكلمة المرور");
        //        }


        //        using (var trans = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (user.Status == 0)
        //                {
        //                    user.Status = 1;
        //                }
        //                user.Password = Security.ComputeHash(userActivate.password, HashAlgorithms.SHA512, null);
        //                db.SaveChanges();
        //                trans.Commit();
        //                return Ok("لقد قمت بتغير كلمة المرور بنجاح");
        //            }
        //            catch (Exception)
        //            {
        //                trans.Rollback();
        //                return StatusCode(500, null);

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, null);
        //    }

        //}


        public string GetResetPasswordHTML(string UserName, string path)
        {

            string WebServer = Configuration.GetSection("Links")["WebServer"],
                   EmailSupport = Configuration.GetSection("Links")["EmailSupport"];


            return "<!DOCTYPE html>" +
                   "<html lang = \"ar\" dir = \"rtl\"><head><meta charset = \"UTF-8\"><style>" +
                   "div.wrapper{ margin: auto; margin-top:13vh; max-width:550px; }" +
                   "img{ width: 100 %; height: 45px; } footer{ width: 85 %; margin: auto; }" +
                    "p{ line-height: 1.4; text-align: justify; }" +
                  ".grey{ color: grey; }.padd{padding: 10px 5px; }" +
                  ".Helvetica{ font-family: Helvetica; font-size: 14.5px; }" +
                  "footer div { text-align: center; }" +
                  "body{ font-family: Arial; font-size:15.5px; }" +
                  "</style></head><body>" +
                  "<div class=\"wrapper\"><header><img src = \"" + WebServer + "/img/ddd.png\" /></header><div class=\"padd\"><p>عزيزي المستخدم<span>" + " " + UserName + " " + "</span></p>" +
                  "<p>  لتتمكن من استرجاع حسابك عليك ادخال كلمة مرور جديدة عن طريق النقر على الرابط أدناه :</p> " +
                 "<p>الرابط: <a href = \" " + WebServer + path + "\" > Click Here</a></p>" +
                 "<br><p>فريق عمل مشروع<b>مصلحة الاحوال المدنية</b> </p>" +
                "</div><footer class=\"Helvetica\"><div class=\"grey\"><a href = \"" + WebServer + "\"> visit our website</a> | <a href = \"" + WebServer + "\"> log in to your account</a> | <a href = \"mailto:" + EmailSupport + "\"> get support</a></div>" +
                "<div class=\"grey\"> All rights reserved ,مشروع مصلحة الاحوال المدنية  Copyright © CRA</div></footer></div></body></html>";

        }

        public class UploadExcelObject
        {

            public string photo { get; set; }
        }

        public long SetUpNID(object nid)
        {
            try
            {
                if (nid == null)
                    return 0;

                if (string.IsNullOrEmpty(nid.ToString()))
                    return 0;

                string a = nid.ToString();
                string b = string.Empty;

                for (int i = 0; i < a.Length; i++)
                {
                    if (Char.IsDigit(a[i]))
                        b += a[i];
                }

                if (b.Length > 0)
                {
                    return long.Parse(b);
                }
                else
                {
                    return long.Parse(nid.ToString());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public string replaceEmpty(object input)
        {
            try
            {
                if (input == null)
                    return "";

                return input.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public int replaceEmptyInt(object input)
        {
            try
            {
                if (input == null)
                    return 0;

                if (!IsDigitsOnly(input.ToString()))
                    return 0;

                return int.Parse(input.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        public long replaceEmptyLong(object input)
        {
            try
            {
                if (input == null)
                    return 0;


                if (!IsDigitsOnly(input.ToString()))
                    return 0;

                return long.Parse(input.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public short replaceEmptyShort(object input)
        {
            try
            {
                if (input == null)
                    return 0;


                if (!IsDigitsOnly(input.ToString()))
                    return 0;

                return short.Parse(input.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public string getExamName(int number)
        {

            switch (number)
            {
                case 1:
                    return "امتحان الباب الأول";
                case 2:
                    return "امتحان الباب التاني";
                case 3:
                    return "امتحان الباب التالث";
                case 4:
                    return "امتحان الباب الرابع";
                case 5:
                    return "امتحان الباب الخامس";
                case 6:
                    return "امتحان الباب السادس";
                case 7:
                    return "امتحان الباب السابع";
                case 8:
                    return "امتحان الباب التامن";
                case 9:
                    return "امتحان الباب التاسع";
                case 10:
                    return "امتحان الباب العاشر";
                case 11:
                    return "امتحان الباب الحادي عشر";
                case 12:
                    return "امتحان الباب التاني عشر";
                case 13:
                    return "امتحان الباب التالث عشر";
                case 14:
                    return "امتحان الباب الرابع عشر";
                case 15:
                    return "امتحان الباب الخامس عشر";
                case 16:
                    return "امتحان الباب السادس عشر";
                case 17:
                    return "امتحان الباب السابع عشر";
                case 18:
                    return "امتحان الباب التامن عشر";
                case 19:
                    return "امتحان الباب التاسع عشر";
                case 20:
                    return "امتحان الباب العشرون";

                default:
                    return "امتحان غير معروف ";

            }

        }

        //public class ExistExam
        //{
        //    public Exams exams { get; set; }
        //    public int Number { get; set; }
        //}

        //[HttpPost]
        //public IActionResult UploadExcel([FromBody] UploadExcelObject excel)
        //{
        //    var userId = this.help.GetCurrentUser(HttpContext);
        //    if (userId <= 0)
        //    {
        //        return StatusCode(401, "الرجاء تسجيل الدخول");
        //    }

        //    int ErrorRowNumber = 0;
        //    int ErorPalce = 0;

        //    var examDetails = new List<ExamDetails>();
        //    var erorrRows = new List<ErorrRow>();
        //    var examAnswers = new List<ExamAnswers>();

        //    List<ExistExam> existExamList = new List<ExistExam>();


        //    try
        //    {
        //        byte[] Excel = Convert.FromBase64String(excel.photo.Substring(excel.photo.IndexOf(",") + 1));
        //        using (MemoryStream memStream = new MemoryStream(Excel))
        //        {
        //            ExcelPackage package = new ExcelPackage(memStream);
        //            ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //            ExamAnswers examAnswer = new ExamAnswers();

        //            //ExcelWorksheet workSheet = package.Workbook.Worksheets["احتياط 2020"];
        //            //ExcelWorksheet workSheet = package.Workbook.Worksheets["الاحتياط العام 2020"];
        //            ExcelWorksheet workSheet = package.Workbook.Worksheets["Questions"];
        //            int totalRows = workSheet.Dimension.Rows;
        //            int totalColumns = workSheet.Dimension.Columns;


        //            var sectionId = (from p in db.MoeSections where p.SectionNumber == replaceEmpty(workSheet.Cells[3, 2].Value).ToString() select p.Id).FirstOrDefault();


        //            if (sectionId <= 0)
        //            {
        //                return StatusCode(404, "الرجاء التاكد من الملف واعادة المحاولة");
        //            }

        //            if (totalColumns <= 0)
        //            {
        //                return StatusCode(404, "لايحتوي الملف على بيانات");
        //            }

        //            int examNumber = 0;
        //            int examNumberNew = 0;
        //            int rows = 0;
        //            int ErorrRows = 0;

        //            Exams exams = new Exams();

        //            for (int i = 3; i <= totalRows; i++)
        //            {


        //                // To Get 

        //                var QType = replaceEmptyShort(workSheet.Cells[i, 5].Value);

        //                if (QType == 2 ||
        //                    QType == 4 ||
        //                    QType == 0 ||
        //                    replaceEmptyShort(workSheet.Cells[i, 3].Value) == 0 ||
        //                    (replaceEmpty(workSheet.Cells[i, 6].Value) == "" &&
        //                    replaceEmpty(workSheet.Cells[i, 7].Value) == "")
        //                    )
        //                {
        //                    erorrRows.Add(new ErorrRow()
        //                    {

        //                        Index = replaceEmpty(i).ToString(),
        //                        Year = replaceEmpty(workSheet.Cells[i, 1].Value).ToString(),
        //                        ExamNumber = replaceEmpty(workSheet.Cells[i, 2].Value).ToString(),
        //                        DoorNumber = replaceEmpty(workSheet.Cells[i, 3].Value).ToString(),
        //                        QuestionNumber = replaceEmpty(workSheet.Cells[i, 4].Value).ToString(),
        //                        Question = replaceEmpty(workSheet.Cells[i, 5].Value).ToString(),
        //                        ChoiceNumber = replaceEmpty(workSheet.Cells[i, 6].Value).ToString(),
        //                        Answer1 = replaceEmpty(workSheet.Cells[i, 7].Value).ToString(),
        //                        Answer2 = replaceEmpty(workSheet.Cells[i, 8].Value).ToString(),
        //                        Answer3 = replaceEmpty(workSheet.Cells[i, 9].Value).ToString(),
        //                        Answer4 = replaceEmpty(workSheet.Cells[i, 10].Value).ToString(),
        //                        Answer5 = replaceEmpty(workSheet.Cells[i, 11].Value).ToString(),
        //                        Answer6 = replaceEmpty(workSheet.Cells[i, 12].Value).ToString(),
        //                        TrueAnswer = replaceEmpty(workSheet.Cells[i, 13].Value).ToString(),
        //                        Gearde = replaceEmpty(workSheet.Cells[i, 14].Value).ToString(),

        //                    });


        //                    ErorrRows++;


        //                    continue;
        //                }

        //                examNumber = replaceEmptyShort(workSheet.Cells[i, 3].Value);
        //                rows++;

        //                bool isExist = false;

        //                if (examNumber != examNumberNew)
        //                {
        //                    foreach (ExistExam item in existExamList)
        //                    {
        //                        if (item.Number == examNumber)
        //                        {
        //                            exams = item.exams;
        //                            isExist = true;
        //                        }

        //                    }

        //                    if (!isExist)
        //                    {
        //                        exams = new Exams();
        //                        examNumberNew = examNumber;
        //                        exams.ExamName = getExamName(replaceEmptyShort(workSheet.Cells[i, 3].Value));
        //                        exams.MoeSectionId = sectionId;
        //                        exams.Status = 1;
        //                        exams.CreatedBy = userId;
        //                        exams.CreatedOn = DateTime.Now;

        //                        db.Exams.Add(exams);

        //                        ExistExam exist = new ExistExam();
        //                        exist.exams = exams;
        //                        exist.Number = replaceEmptyShort(workSheet.Cells[i, 3].Value);
        //                        existExamList.Add(exist);
        //                    }

        //                }

        //                ErorPalce = i;




        //                examAnswer.Answer = replaceEmpty(workSheet.Cells[i, 8].Value);

        //                if (replaceEmptyShort(workSheet.Cells[i, 5].Value) == 3)
        //                {
        //                    examAnswer.AnswerNumber = 1;
        //                }
        //                else if (replaceEmptyShort(workSheet.Cells[i, 5].Value) == 1)
        //                {
        //                    examAnswer.AnswerNumber = replaceEmptyShort(workSheet.Cells[i, 14].Value);
        //                }

        //                examAnswer.Status = 1;
        //                examAnswer.CreatedBy = userId;
        //                examAnswer.CreatedOn = DateTime.Now;
        //                examAnswers.Add(examAnswer);
        //                examDetails.Add(new ExamDetails()
        //                {

        //                    Questions = replaceEmpty(workSheet.Cells[i, 6].Value),
        //                    QuestionType = replaceEmptyShort(workSheet.Cells[i, 5].Value),
        //                    Status = 1,
        //                    CreatedBy = userId,
        //                    CreatedOn = DateTime.Now,
        //                    ExamId = exams.ExamId,
        //                    ExamAnswers = examAnswers,

        //                    //Degree = double.Parse(workSheet.Cells[i, 8].Value.ToString()),
        //                });

        //                db.ExamDetails.AddRange(examDetails);

        //            }

        //            db.SaveChanges();

        //            CreateExcelFile(ExcelHelper.ToDataTable(erorrRows));



        //            return Ok("تم تحميل البيانات بنجاح عدد البيانات هو  " + rows + " الغير مدخلة" + ErorrRows);



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("*************************************");
        //        Console.WriteLine("*************************************");
        //        Console.WriteLine(ErorPalce);
        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine("*************************************");
        //        Console.WriteLine("*************************************");
        //        return StatusCode(404, ex.Message + "خطا في الصف :" + ErrorRowNumber);
        //    }
        //}






        //public static void CreateExcelFile(DataTable table, string destination = @"D:\\الاسئلىة الغير مدخلة.xlsx")
        //{
        //    using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
        //    {
        //        var workbookPart = workbook.AddWorkbookPart();

        //        workbook.WorkbookPart.Workbook = new Workbook
        //        {
        //            Sheets = new Sheets()
        //        };

        //        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();

        //        var sheetData = new SheetData();

        //        sheetPart.Worksheet = new Worksheet(sheetData);

        //        Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();

        //        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

        //        uint sheetId = 1;

        //        if (sheets.Elements<Sheet>().Count() > 0)
        //        {
        //            sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
        //        }

        //        Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };

        //        sheets.Append(sheet);

        //        Row headerRow = new Row();

        //        List<String> columns = new List<string>();

        //        foreach (DataColumn column in table.Columns)
        //        {
        //            columns.Add(column.ColumnName);

        //            Cell cell = new Cell
        //            {
        //                DataType = CellValues.String,

        //                CellValue = new CellValue(column.ColumnName),
        //            };

        //            headerRow.AppendChild(cell);
        //        }

        //        sheetData.AppendChild(headerRow);

        //        // Excel row character .
        //        var strCellRange = new List<string>()
        //        {
        //            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V"
        //        };

        //        var refernceCount = 1;

        //        foreach (DataRow dsrow in table.Rows)
        //        {
        //            Row newRow = new Row();

        //            refernceCount += 1;

        //            var coulumnIndex = 0;

        //            foreach (String col in columns)
        //            {

        //                Cell cell = new Cell
        //                {
        //                    DataType = CellValues.String,

        //                    CellValue = new CellValue(dsrow[col].ToString()),

        //                    CellReference = strCellRange[coulumnIndex] + refernceCount
        //                };


        //                newRow.AppendChild(cell);

        //                coulumnIndex += 1;
        //            }

        //            sheetData.AppendChild(newRow);
        //        }
        //    }
        //}


    }
}