using CIPlatformMain.Entities.Data;
using Microsoft.AspNetCore.Mvc;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Entities.Models;
using System.Text.RegularExpressions;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using CIPlatformMain.Repository.Interface;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CIPlatformMain.Controllers
{
    public class StoryController : Controller
    {
        private readonly CidatabaseContext _cidatabase;
        private readonly IStory _istory;

        public StoryController( CidatabaseContext cidatabase, IStory istory)
        {
            _cidatabase = cidatabase;
            _istory = istory;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public JsonResult GetStory(long MissionId)
        {
            var userid = long.Parse(HttpContext.Session.GetString("UserID"));
            var storydata = _istory.GetStoryData();
            Story story= storydata.story.Where(s=>s.MissionId == MissionId && s.UserId==userid && s.Status=="DRAFT" && s.DeletedAt==null).FirstOrDefault();
            
            
            if (story != null)
            {
                var images = _cidatabase.StoryMedia.Where(m => m.StoryId == story.StoryId).ToList();
               
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 1024
                };
                var json = System.Text.Json.JsonSerializer.Serialize(story, options);


                return Json(json);
            }
            else
            {
                return Json("NoStoryFound");
            }
        }
       
        //AddEdit story Get Method
        public IActionResult AddEditStory()
        {
            var userid = long.Parse(HttpContext.Session.GetString("UserID"));
            var storydata = _istory.GetStoryData();
            storydata.Applications = storydata.Applications.Where(a => a.UserId == userid).ToList();
           
            return View(storydata);
        }


        //To Add and Edit StoryData 
        [HttpPost]
    
        public IActionResult AddEditStoryPost(long StoryId, long MissionId,string StoryDescription,DateTime StoryDate,string StoryTitle,List<IFormFile> StoryImages,string StoryVideoURL ,List<string> Preloaded,string Status)
        {



            if (MissionId == 0 || StoryDate == null || StoryTitle == null)
            {
                TempData["EmptyField"] = 1;
                return View();
            }
            else
            {
                TempData["EmptyField"] = null;
                var userid = long.Parse(HttpContext.Session.GetString("UserID"));
                var storydata = _istory.GetStoryData();
                var story = storydata.story.Where(s => s.MissionId == MissionId && s.UserId == userid && s.Status == "DRAFT").FirstOrDefault();
                if (story == null)
                {
                    if (StoryId == 0)
                    {
                        var status = _istory.AddStory(userid, MissionId, StoryDescription, StoryDate, StoryTitle, StoryImages, StoryVideoURL, Status);
                        if (status == true)
                        {
                            return Json("Success");

                        }
                        else
                        {
                            return Json("Error");
                        }
                    }
                    else
                    {
                        var status = _istory.EditStory(StoryId, userid, MissionId, StoryDescription, StoryDate, StoryTitle, StoryImages, StoryVideoURL, Preloaded, Status);
                        if (status == true)
                        {
                            return Json("Success");

                        }
                        else
                        {
                            return Json("Error");
                        }
                    }
                }
                else
                {
                    var status = _istory.EditStory(StoryId, userid, MissionId, StoryDescription, StoryDate, StoryTitle, StoryImages, StoryVideoURL, Preloaded, Status);
                    if (status == true)
                    {
                        return Json("Success");

                    }
                    else
                    {
                        return Json("Error");
                    }
                }
               
                
            }
        }
        //To Show Stories
        public IActionResult StoryListPage(int pg = 1)
        {
          //  ViewData["Name"] = HttpContext.Session.GetString("Username");

            var storydata = _istory.GetStoryData();
            storydata.story = storydata.story.Where(s => s.Status == "PUBLISHED").ToList();


            //To Pagination
            const int pageSize = 3;
            if (pg < 1)
                pg = 1;

            int recsCount = storydata.story.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = storydata.story.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            storydata.story = data.ToList();

            
            return View(storydata);
        }

  
      
        public IActionResult StoryDetails(int storyid)
        {
            var story = _cidatabase.Stories.Where(s=>s.StoryId==storyid).FirstOrDefault();
            if (story != null)
            {
                if (story.Views != null)
                {
                    story.Views = story.Views + 1;
                    _cidatabase.Update(story);
                    _cidatabase.SaveChanges();
                }
                else
                {
                    story.Views = 1;
                    _cidatabase.Update(story);
                    _cidatabase.SaveChanges();
                }
            }
            
           
            var storydata = new StoryData
            {
                story = _cidatabase.Stories.Where(s=>s.StoryId==storyid).ToList(),
                storyMedia = _cidatabase.StoryMedia.Where(s => s.StoryId == storyid).ToList(),
                user = _cidatabase.Users.ToList(),
                mission=_cidatabase.Missions.ToList()
            };
            ViewData["UserAvatar"] = storydata.user.Where(u => u.UserId == long.Parse(HttpContext.Session.GetString("UserID"))).Select(u => u.Avatar).FirstOrDefault();



            return View(storydata);
        }

        public JsonResult GetDraftStory(int missionid)
        {
            var userid = long.Parse(HttpContext.Session.GetString("UserID"));

            var Story = _cidatabase.Stories.Where(s => s.UserId == userid && s.MissionId == missionid && s.Status == "DRAFT").FirstOrDefault();
            if (Story == null)
            {
                RedirectToAction("ShareYourStoryAdd");
                var draftDetails = new { title = "", description = "", date = ""};
                return Json(draftDetails);

            }
            else
            {
                var storyMedia = _cidatabase.StoryMedia.Where(m => m.StoryId == Story.StoryId).ToList();
                
                
                IEnumerable<string> paths = storyMedia.Select(m => m.Path).ToList();

                var draftDetails = new { title = Story.Title, description = Story.Description, path = paths, date = Story.PublishedAt };
                return Json(draftDetails);
            }



        }

        public RedirectResult Recommend(int StoryId, int to_userid)
        {
            var to_useremail = _cidatabase.Users.Where(u => u.UserId == to_userid).Select(u => u.Email).SingleOrDefault();

            var userid = long.Parse(HttpContext.Session.GetString("UserID"));
            if (to_useremail != null)
            {
            

                var mailbody = "<h1>Watch Story</h1><br><h2><a href='" + "https://localhost:7037/Story/StoryDetails?storyid=" + StoryId + "'></a></h2>";
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("ciplatformmailsenderchintan@gmail.com"));
                email.To.Add(MailboxAddress.Parse(to_useremail));
                email.Subject = "Story Invite";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = mailbody };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("ciplatformmailsenderchintan@gmail.com", "pmjfsnfycrhnygrw");
                smtp.Send(email);
                smtp.Disconnect(true);


              StoryInvite storyInvite = new StoryInvite();
                storyInvite.StoryId = StoryId;
                storyInvite.CreatedAt = DateTime.Now;
                storyInvite.FromUserId = userid;
                storyInvite.ToUserId = to_userid;


                _cidatabase.Add(storyInvite);
                _cidatabase.SaveChanges();



            }


            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }

    }
}
