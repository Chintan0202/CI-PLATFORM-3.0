using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Models;
using CIPlatformMain.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Diagnostics;

namespace CIPlatformMain.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin _iadmin;
        public AdminController(IAdmin iadmin)
        {
            _iadmin = iadmin;

        }


        //<------------------------------------------Methods For Admin------------------------------------------------->

        //Admin Login [Get] Method
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }


        //To Login to admin account
        [HttpPost]
        public IActionResult AdminLogin(Admin admin)
        {

            var result = _iadmin.AdminLogin(admin);

            if (result == true)
            {
                HttpContext.Session.SetString("AdminId", admin.AdminId.ToString());
                return RedirectToAction("AdminPage");
               
                
            }
            
            else
            {
                
                return View(admin);
            }

        }
       
        //To Logout from Admin Account
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminId");
            return RedirectToAction("AdminLogin");
        }

       
        //To get Data For Admin Screens
        public IActionResult AdminPage()
        {
            if (HttpContext.Session.GetString("AdminId") != null)
            {
                var Data = _iadmin.GetData();
                return View(Data);
            }
            else
            {

                return RedirectToAction("Index","Home");
            }

        }

      
        //To Get UserData
        public IActionResult UserData()
        {
            var Data = _iadmin.GetData();
            if (TempData["CMSPageRedirect"] != null)
            {
                return PartialView("_CMSPage", Data);
            }
            else
            {
                return PartialView("_User", Data);
            }
           

            
        }


        //<------------------------------------------Methods For CMSPage CRUD------------------------------------------------->

        //To Get CMSList
        public IActionResult CMSList()
        {
            var Data = _iadmin.GetData();
            return PartialView("_CMSPage", Data);
        }

        //To AddEdit CMSPage [Get Method]
        public IActionResult AddEditCMSPage(long CMSId)
        {
            var Data = _iadmin.GetData();
            var CMSData=Data.CmsPage;
            if (CMSId != null)
            {
                CMSData = Data.cmsPages.Where(c => c.CmsPageId == CMSId).FirstOrDefault();
            }
            
            return PartialView("_AddEditCMSPage", CMSData);
        }

        //To AddEdit CMSPage [Post Method]       
        [HttpPost]
        public IActionResult AddEditCMSPage(CmsPage cmsPage)
        {
           
                if (cmsPage.CmsPageId !=0)
                {
                    _iadmin.EditCMSPage(cmsPage, cmsPage.CmsPageId);
                }
                else
                {
                    _iadmin.AddCMSPage(cmsPage);
                }


            
            TempData["CMSPageRedirect"] = "CMSRedirect";
            return RedirectToAction("AdminPage");


        }

        //To Delete CMSPage
        public IActionResult DeleteCMSPage(long CMSId)
        {
            var status = _iadmin.DeleteCMSPage(CMSId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }
        }






        //<------------------------------------------Methods For Skill CRUD------------------------------------------------->


        //To Get SkillList
        public IActionResult MissionSkillList()
        {
            var Data = _iadmin.GetData();
            Data.Skills = Data.Skills.Where(s => s.DeletedAt == null).ToList();
            return PartialView("_MissionSkills", Data);
        }

        //To Draft Skill For Edit
        public IActionResult DraftSkill(long SKillId)
        {
            if (SKillId != 0)
            {
                var skill = _iadmin.GetSkill(SKillId);
                return Json(skill);
            }
            else
            {
                return Json("Add");
            }

            
        }

        //To Edit Skill
        public IActionResult AddEditSkill(long SkillId,string SkillName,int SkillStatus)
        {
            if (SkillId != 0)
            {
                var status = _iadmin.EditSkill(SkillId, SkillName, SkillStatus);
                if (status == true)
                {
                    TempData["DeleteStatus"] = 1;
                    return RedirectToAction("AdminPage");
                }
                else
                {
                    TempData["DeleteStatus"] = null;
                    return RedirectToAction("AdminPage");
                }
            }
            else
            {
                var status = _iadmin.AddSkill(SkillName, SkillStatus);
                if (status == true)
                {
                    TempData["DeleteStatus"] = 1;
                    return RedirectToAction("AdminPage");
                }
                else
                {
                    TempData["DeleteStatus"] = null;
                    return RedirectToAction("AdminPage");
                }
            }

            
        }

        //To Delete Skill       
        public IActionResult DeleteSkill(long SkillId)
        {
            var status=_iadmin.DeleteSkill(SkillId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }
        }



        //<------------------------------------------Methods For Story CRUD------------------------------------------------->


        //To Get  StoryList
        public IActionResult StoryList()
        {
            var Data = _iadmin.GetData();
            Data.Stories = Data.Stories.Where(s => s.Status == "PENDING" && s.DeletedAt==null).ToList();
            return PartialView("_StoryList", Data);
        }

        //To  Get Details For Story
        public IActionResult StoryDetails(long StoryId)
        {
            var Data = _iadmin.GetData();
            Data.Story=Data.Stories.Where(s=>s.StoryId==StoryId).FirstOrDefault();
            Data.storyMedia=Data.AllstoryMedia.Where(m=>m.StoryId==StoryId).ToList();
            return PartialView("_viewStory", Data);
        }

        //To Decline Request For Story Publish
        public IActionResult StoryDecline(long StoryId)
        {
           var status=_iadmin.StoryDecline(StoryId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }
            
        }

       //To Aprove Story Publish Request
        public IActionResult StoryApprove(long StoryId)
        {
            var status=_iadmin.StoryApprove(StoryId);
            if (status == true)
            {
                TempData["ApproveStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["ApproveStatus"] = null;
                return RedirectToAction("AdminPage");
            }
            
        }
        
        //To Delete Story Post Request 
        public IActionResult StoryDelete(long StoryId) {

            var status=_iadmin.StoryDelete(StoryId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }
            
        }


        //<------------------------------------------Methods For Mission CRUD------------------------------------------------->


        //To Get Mission
        public IActionResult MissionList()
        {
            var Data = _iadmin.GetData();
            return PartialView("_MissionList", Data);
        }

        //To Delete Mission
        public IActionResult DeleteMission(long MissionId)
        {
            var status=_iadmin.DeleteMission(MissionId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }
        }
        
      
        //To AddEdit Mission [GET Method]
        public IActionResult AddEditMission(long MissionId)
        {
            AddMission addMission = _iadmin.getMissionData();
            
            if (MissionId != 0)
            {
                addMission.Mission=addMission.Missions.Where(m=>m.MissionId==MissionId).FirstOrDefault();
                addMission.MissionMediums = addMission.MissionMediums.Where(m => m.MissionId == MissionId).ToList();
                addMission.MissionsDocuments = addMission.MissionsDocuments.Where(m => m.MissionId == MissionId).ToList();
                addMission.MissionSkills = addMission.MissionSkills.Where(m => m.MissionId == MissionId).ToList();
                addMission.GoalMission = addMission.GoalMissions.Where(m => m.MissionId == MissionId).FirstOrDefault();
            }
            
            return PartialView("_AddEditMission", addMission);
        }
        
        
        //To AddEdit Mission [POST method]
        [HttpPost]
        public IActionResult AddEditMission(Mission mission,List<IFormFile> MissionDocument, List<IFormFile> MissionPhotos, List<IFormFile> MissionImages, IFormFile DefualtMissionPhotos, String MissionVideoURL,List<int> SkillList,string preloadedImages,GoalMission goalMission)
        {

            if (mission.MissionId != 0)
            {

                var status=_iadmin.EditMission(mission, MissionPhotos, DefualtMissionPhotos, MissionDocument, MissionVideoURL, SkillList, preloadedImages, goalMission);
                TempData["MissionListRedirect"] = 1;
            }
            else
            {


                var status = _iadmin.AddMission(mission, MissionPhotos, DefualtMissionPhotos, MissionDocument, MissionVideoURL, SkillList, goalMission);
                TempData["MissionListRedirect"] = 1;
            }
                return RedirectToAction("AdminPage");
        }

        public JsonResult GetCities(int Country_id)
        {
            try
            {
                var cities = _iadmin.getCities(Country_id);
                return Json(cities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        //<------------------------------------------Methods For Theme CRUD------------------------------------------------->


        //To Get ThemeList
        public IActionResult MissionThemeList()
        {
            var Data = _iadmin.GetData();
            return PartialView("_MissionTheme", Data);
        }


        //To Draft Theme For Edit
        public IActionResult DraftTheme(long MissionThemeId)
        {

            if (MissionThemeId!=0)
            {
                MissionTheme missionTheme = _iadmin.GetMissionTheme(MissionThemeId);
                return Json(missionTheme);
            }
            else
            {
                return Json("Add");
            }
          
        }
        

        //To AddEdit Theme
        public IActionResult AddEditTheme(string ThemeName, int Status, long ThemeId)
        {
            if (ThemeId == 0)
            {
                var status=_iadmin.AddTheme(ThemeName, Status);
                if (status == true)
                {
                    TempData["Addtheme"] = 1;
                }
                else
                {
                    TempData["Addtheme"] = null;
                }

            }
            else
            {
                var status = _iadmin.EditTheme(ThemeName, Status, ThemeId);
                if (status == true)
                {
                    TempData["Edittheme"] = 1;
                }
                else
                {
                    TempData["Edittheme"] = null;
                }
                
            }
            return RedirectToAction("AdminPage");
        }
       
        //To Delete Theme
        public IActionResult DeleteTheme(long MissionThemeId)
        {
            var status = _iadmin.DeleteTheme(MissionThemeId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }
        }



        //<------------------------------------------Methods For USER CRUD------------------------------------------------->


        public IActionResult AddUser()
        {

            var Data = _iadmin.GetData();
            ViewData["countries"] = Data.Country;
            ViewData["cities"] = Data.City;
            return View();
        }

        [HttpPost]
        public IActionResult AddUserData(User _user,IFormFile Avatar)
        {
            var emailstatus= _iadmin.CheckEmailInDatabase(_user);
            if (emailstatus == 1)
            {
                
                   
                TempData["EmailStatus"] = null;
                
                
            }
            else
            {
                TempData["EmailStatus"] = 1 ;
            }

            
            return RedirectToAction("AdminPage");
        }

       
        //Get Method Of user AddEdit
        public IActionResult AddEditUserPage(long UserId)
        {
            var Data = _iadmin.GetData();
           
            if (UserId != null)
            {
                Data.User = Data.Users.Where(c => c.UserId == UserId).FirstOrDefault();
            }

            return PartialView("_AddEditUserPage", Data);
        }

       
        //To AddEdit User POST Method
        [HttpPost]
        public IActionResult AddEditUserPage(User user,IFormFile Avatar)
        {

            
            if (user.UserId != 0)
            {
                var emailstatus = _iadmin.CheckEmailInDatabase(user);
                if (emailstatus == 1)
                {
                    _iadmin.EditUser(user, user.UserId, Avatar);
                    TempData["EmailStatus"] = null;
                }
                else
                {
                    TempData["EmailStatus"] = 1;
                }
                
            }
            else
            {
                var emailstatus = _iadmin.CheckEmailInDatabase(user);
                if (emailstatus == 1)
                {
                    _iadmin.AddUser(user, Avatar);
                    TempData["EmailStatus"] = null;
                }
                else
                {
                    TempData["EmailStatus"] = 1;
                }
                
            }
            

           
            return RedirectToAction("AdminPage");


        }
      
        //To DeleteUser
        public IActionResult DeleteUser(long UserId)
        {
            var DeleteStatus=_iadmin.DeleteUser(UserId);
            if(DeleteStatus == true)
            {
                TempData["DeleteStatus"] = 1;
            }
            else
            {
                TempData["DeleteStatus"] = null;
            }
            return RedirectToAction("AdminPage");
        }


        //<------------------------------------------Methods For MissionApplication CRUD------------------------------------------------->


        //To Get Mission Application List
        public IActionResult MissionApplicationList()
        {
            var Data = _iadmin.GetData();
            Data.MissionApplications = Data.MissionApplications.Where(a => a.DeletedAt == null).ToList();
            return PartialView("_MissionApplicationList", Data);
        }
      
        //To Approved MissionApplication
        public IActionResult ApproveMissionApplication(long MissionId,long UserId)
        {

            var status = _iadmin.ApproveMissionApplication( MissionId,UserId);
            if(status == true)
            {
                TempData["MissionApplicationStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["MissionApplicationStatus"] = null;
                return RedirectToAction("AdminPage");
            }
            
        }
      
        //To Delete MissionApplication
        public IActionResult DeclineMissionApplication(long MissionId, long UserId)
        {

            var status = _iadmin.DeclineMissionApplication(MissionId, UserId);
            if (status == true)
            {
                TempData["DeleteStatus"] = 1;
                return RedirectToAction("AdminPage");
            }
            else
            {
                TempData["DeleteStatus"] = null;
                return RedirectToAction("AdminPage");
            }

        }



        //<------------------------------------------Methods For BannerManagment CRUD------------------------------------------------->


        //To Get BannersList
        public IActionResult GetBannerList()
        {
            var Data = _iadmin.GetData();
            return PartialView("_BannerManagment", Data);
        }
      
        //To Get Banner For Editing
        public JsonResult DraftBanner(long BannerId)
        {
            try
            {
                if (BannerId != null)
                {
                    Banner banner = _iadmin.GetBanner(BannerId);
                    if (banner != null)
                    {
                        return Json(banner);
                    }
                    else
                    {
                        return Json("Not Found");
                    }
                }
                else
                {
                    return Json("NotFound");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Processing failed: {ex.Message}");
                return Json("NotFound");
            }
        }


        public IActionResult DeleteBanner(long BannerId)
        {

            if (BannerId != null)
            {
                _iadmin.DeleteBanner(BannerId);
                return Json("Success");
            }
            else
            {
                return Json("Failure");
            }
            
        }
       
        //To Add/Edit Banners
        public IActionResult AddEditBanner(Banner BannerData, IFormFile BannerImage)
        {
            if (BannerData.BannerId != 0)
            {
                var BannerId = BannerData.BannerId;
                var status=_iadmin.EditBanner(BannerId,BannerData, BannerImage);
                if (status == true)
                {

                    TempData["BannerManagmentRediect"] = 1;
                    return RedirectToAction("AdminPage");

                }
                else
                {
                    TempData["BannerManagmentRediect"] = null;
                    return RedirectToAction("AdminPage");
                }
            }
            else
            {

                var status = _iadmin.AddBanner(BannerData, BannerImage);
                if (status == true)
                {

                    TempData["BannerManagmentRediect"] = 1;
                    return RedirectToAction("AdminPage");

                }
                else
                {
                    TempData["BannerManagmentRediect"] = null;
                    return RedirectToAction("AdminPage");
                }
            }

          
        }
    }
}
