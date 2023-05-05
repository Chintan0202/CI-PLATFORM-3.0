using CIPlatformMain.Entities.Data;
using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using MimeKit;
using NuGet.Common;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

using System.Drawing.Printing;
using System.Drawing;
using NuGet.Protocol;
using System.Linq;
using CIPlatformMain.Repository.Interface;

namespace CIPlatformMain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CidatabaseContext _cidatabase;
        private readonly IUser _iuser;
        private readonly IHome _ihome;

        public HomeController(ILogger<HomeController> logger, CidatabaseContext cidatabase, IUser iuser, IHome ihome)
        {
            _iuser = iuser;
            _ihome = ihome;
            _logger = logger;
            _cidatabase = cidatabase;
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.resetstatus = TempData["Resetstatus"];
            ViewData["Banners"] = _ihome.GetBanners();
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("Login");
        }



        [HttpPost]
        public IActionResult Login(User _user)
        {
            ViewData["Banners"] = _ihome.GetBanners();
            User User = _iuser.Login(_user);

            
            var Deletestatus = _iuser.DeletedUser(_user);
            if (User != null )
            {

                HttpContext.Session.SetString("Username", User.FirstName);
                HttpContext.Session.SetString("UserID", User.UserId.ToString());
                HttpContext.Session.SetString("UserEmail", User.Email);

                if (User.Avatar != null)
                {
                    HttpContext.Session.SetString("UserAvatar", User.Avatar);
                }
                else
                {
                    HttpContext.Session.SetString("UserAvatar", " ");
                }
               


                TempData["logintoast"] = 1;
                return RedirectToAction("LandingPage");

            }
           
            else if (Deletestatus != null && Deletestatus.DeletedAt != null)
            {
                ViewBag.Logincredentials = 1;
                return View();
            }
            else
            {
                ViewBag.Logincredentials = 0;
                return View(_user);
            }
           
        }

        public IActionResult Registration()
        {
            ViewData["Banners"] = _ihome.GetBanners();
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User _user)
        {


            ViewData["Banners"] = _ihome.GetBanners();
            /* var status = _cidatabase.Users.Where(m => m.Email == _user.Email).FirstOrDefault();*/
            var status = _iuser.CheckEmailInDatabase(_user);
            if (status == 1)
            {

                if (ModelState.IsValid)
                {

                    _iuser.Register(_user);
                    TempData["Registeredstatus"] = 1;
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "User with same email-id already exists");
                return View();
            }
            return View();
        }
     
        
        [HttpGet]
        public IActionResult Forgotpassword()
        {
            ViewData["Banners"] = _ihome.GetBanners();
            return View();
        }
        [HttpPost]
        public IActionResult Forgotpassword(String useremail)
        {
            ViewData["Banners"] = _ihome.GetBanners();
            var registereduser = _cidatabase.Users.Where(u => u.Email == useremail).FirstOrDefault();
            if (registereduser == null)
            {
                ViewBag.forgetstatus = 0;
            }
            else
            {
                var token = Guid.NewGuid().ToString();
                var passwordReset = new PasswordReset
                {

                    Email = useremail,
                    Token = token

                };
                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("Email", useremail);
                _cidatabase.PasswordResets.Add(passwordReset);
                _cidatabase.SaveChanges();

                var mailbody = "<h1>Click link to reset password</h1><br><h2><a href='" + "https://localhost:7037/Home/ResetPassword?token=" + token + "'>Reset Password</a></h2>";
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("ciplatformmailsenderchintan@gmail.com"));
                email.To.Add(MailboxAddress.Parse(useremail));
                email.Subject = "Reset Your Password";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = mailbody };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate("ciplatformmailsenderchintan@gmail.com", "pmjfsnfycrhnygrw");
                smtp.Send(email);
                smtp.Disconnect(true);


              
                return View();

            }

            return View();
        }


        public IActionResult LandingPage(string searchin, string themefilter, string cityfilter, string countryfilter,string skillfilter, int sortby, int pg = 1)//change here
        {
           
            
            //First Check If User is Login OR Not
            
               
               
                var missiondata = _ihome.GetMissions();

            long UserId;
            if (HttpContext.Session.GetString("UserID") != null)
            {
                UserId = long.Parse(HttpContext.Session.GetString("UserID"));

               
                missiondata.FavMission = missiondata.FavMission.Where(f => f.UserId == UserId).ToList();
                missiondata.Application = missiondata.Application.Where(u => u.UserId == UserId).ToList();
            }

                //To Sort Mission
                if (sortby > 0)
                {
                    switch (sortby)
                    {

                        //Sortby CreatedAt Decendning
                        case 1:
                            missiondata.Missions = missiondata.Missions.OrderByDescending(p => p.CreatedAt).ToList();

                            break;
                        //Sortby createdAt     
                        case 2:
                            missiondata.Missions = missiondata.Missions.OrderBy(p => p.CreatedAt).ToList();

                            break;
                        //Sortby Mission Startdate
                        case 3:
                            missiondata.Missions = missiondata.Missions.OrderBy(p => p.StartDate).ToList();

                            break;
                        //Sortby Mission Startdate Decendning
                        case 4:
                            missiondata.Missions = missiondata.Missions.OrderByDescending(p => p.StartDate).ToList();

                            break;
                    //Sortby FavList
                    case 5:
                        if (HttpContext.Session.GetString("UserID") != null)
                        {
                            var fav = missiondata.FavMission.Select(m => m.MissionId).ToList();
                            missiondata.Missions = missiondata.Missions.Where(m => fav.Contains(m.MissionId)).ToList();
                        }


                        break;
                    //Sortby SeatsLeft
                    case 6:
                            missiondata.Missions = missiondata.Missions.OrderByDescending(p => p.SeatsLeft).ToList();

                            break;




                    }
                }
                
                //Filters By country
                if (countryfilter != null)
                {
                    missiondata.Missions = missiondata.Missions.Where(m => countryfilter.Contains(m.Country.Name)).ToList();
                    ViewData["countryid"] = missiondata.Country.Where(C => countryfilter.Contains(C.Name)).Select(C => C.CountryId).ToList();

                }

                //Searching Mission
                if (searchin != null)
                {
                    missiondata.Missions = missiondata.Missions.Where(m => m.Title.ToLower().Contains(searchin.ToLower()) || m.Theme.Title.Contains(searchin) || m.Country.Name.Contains(searchin) || m.City.Name.Contains(searchin)).ToList();

                }
                //Filter By Theme
                if (themefilter != null)
                {
                    missiondata.Missions = missiondata.Missions.Where(m => themefilter.Contains(m.Theme.Title)).ToList();

                }
                //Filter By City
                if (cityfilter != null)
                {
                    missiondata.Missions = missiondata.Missions.Where(m => cityfilter.Contains(m.City.Name)).ToList();

                }
                //Filter By skill
                if (skillfilter != null)
                {
                    missiondata.Missions = missiondata.Missions.Where(m => m.MissionSkills.Any(s => skillfilter.Contains(s.Skill.SkillName))).ToList();
                }


                ViewData["MissionCount"] = missiondata.Missions.Count();
               
                
                
                //Pagination Code
                
                const int pageSize = 9;
                if (pg < 1)
                    pg = 1;

                int recsCount = missiondata.Missions.Count();

                var pager = new Pager(recsCount, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = missiondata.Missions.Skip(recSkip).Take(pager.PageSize).ToList();

                this.ViewBag.Pager = pager;

                missiondata.Missions = data.ToList();

                return View(missiondata);
            

        }


        //Parivacy Policy Page View Method
        public IActionResult PrivacyPolicy()
        {
          
            
            return View();
        }

        //404 Error Page For If User Goto Any Page Without Login

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Volunteering_Mission_Page(int missionid, string commenttext, int favvalue, int user_rating, int to_userid)
        {
            
            if (HttpContext.Session.GetString("UserID") != null)
            {
                var userid = long.Parse(HttpContext.Session.GetString("UserID"));
                MissionDetails missiondetails = _ihome.GetMissionDetails(missionid,userid);


                if (missionid > 0)
                {
                    HttpContext.Session.SetString("Mission_ID", missionid.ToString());
                }
 
                if (HttpContext.Session.GetString("Mission_ID") != null)
                {
                    missionid = int.Parse(HttpContext.Session.GetString("Mission_ID"));
                }

                Mission objCategoryList2 = missiondetails.Mission.Where(m => m.MissionId == missionid).FirstOrDefault();


                if (objCategoryList2 != null)
                {
                    var title = objCategoryList2.Title;
                    var missiontheme = objCategoryList2.Theme.Title;
                    var missioncity = objCategoryList2.City.Name;

                }
               

                return View(missiondetails);

            }
            else
            {

                return RedirectToAction("Index");
            }



        }

        //To Add Mission To Favrouite
        public RedirectResult Favrouite(int favvalue,int MissionId)
            {
            var userid = long.Parse(HttpContext.Session.GetString("UserID"));
            var fav_mission_id = MissionId;
           
            ViewData["missionfav"] = _cidatabase.FavoriteMissions.Where(f => f.MissionId == fav_mission_id && f.UserId == userid).Count().ToString();

           
            FavoriteMission favobj = new FavoriteMission();

            if (favvalue == 1)
            {
                if (ViewData["missionfav"] == "0")
                {
                    favobj.MissionId = fav_mission_id;
                    favobj.UserId = long.Parse(HttpContext.Session.GetString("UserID"));
                    _cidatabase.FavoriteMissions.Add(favobj);
                    _cidatabase.SaveChanges();
                }
            }
            if (favvalue == 2)
            {
                var mis = _cidatabase.FavoriteMissions.Where(f => f.MissionId == fav_mission_id && f.UserId == userid).FirstOrDefault();
                _cidatabase.FavoriteMissions.Remove(mis);
                _cidatabase.SaveChanges();
            }
            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
          
        }

        //To Post Comment
        public RedirectResult AddComment(string commenttext)
        {
            var comment_mission_id = int.Parse(HttpContext.Session.GetString("Mission_ID"));
            if (commenttext != null)
            {
                Comment obj = new Comment();
                obj.CommentText = commenttext;
                obj.MissionId = comment_mission_id;
                obj.UserId = long.Parse(HttpContext.Session.GetString("UserID"));
                obj.CreatedAt = DateTime.Now;

                _cidatabase.Comments.Add(obj);
                _cidatabase.SaveChanges();

            }
            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }

        //To Add Ratings
        public RedirectResult Giverating(int user_rating)
        {
            var userid = long.Parse(HttpContext.Session.GetString("UserID"));
            var rating_mission_id = int.Parse(HttpContext.Session.GetString("Mission_ID"));

            var status = _ihome.GiveRating(user_rating, rating_mission_id, userid);

            
            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }

        //To Recommend To Co-Worker
        public RedirectResult Recommend(int missionid, int to_userid)
        {

            var from_userid = long.Parse(HttpContext.Session.GetString("UserID"));
            if (from_userid != 0 && to_userid != 0 && missionid != 0)
            {
                _ihome.Recommend(missionid, from_userid, to_userid);
            }
            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }

        //To Apply To Mission
        public RedirectResult MissionApplication(int missionid)
        {
            var userid = long.Parse(HttpContext.Session.GetString("UserID"));

            var status = _ihome.MissionApplication(missionid, userid);
            if (status == true)
            {
                TempData["ApplicationStatus"] = "true";
            }
            else
            {
                TempData["ApplicationStatus"] = "false";
            }
           

            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }


        //Get Method For ResetPassowrd
        [HttpGet]
        public IActionResult Resetpassword()
        {
            ViewData["Banners"] = _ihome.GetBanners();
            return View();
        }


        //To Reset User Password
        [HttpPost]
        public IActionResult Resetpassword(PasswordReset _user)
        {
            ViewData["Banners"] = _ihome.GetBanners();
            var token = HttpContext.Session.GetString("Token");
            var email = HttpContext.Session.GetString("Email");
            var validatetoken = _cidatabase.PasswordResets.FirstOrDefault(m => m.Token == token);
            if (validatetoken != null)
            {
                var userdata = _cidatabase.Users.Where(m => m.Email == validatetoken.Email).FirstOrDefault();
                userdata.Password = _user.Password;
                _cidatabase.Users.Update(userdata);
                _cidatabase.SaveChanges();
                HttpContext.Session.Remove(token);
                TempData["Resetstatus"] = 1;

                return RedirectToAction("Login");
            }
            TempData["Resetstatus"] = 0;
            return RedirectToAction("Forgotpassword");
        }

        //Contact Use Method
        public RedirectResult Contactus(string User_id, string Subject, string Message)
        {
            var result = false;
            if (!string.IsNullOrEmpty(User_id))
            {
                var user_id = long.Parse(User_id);
                result = _ihome.Contact_us(user_id, Subject, Message);
            }
            if (result != false)
            {
                TempData["result"] = 1;
            }
            else
            {
                TempData["result"] = null;
            }
            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}