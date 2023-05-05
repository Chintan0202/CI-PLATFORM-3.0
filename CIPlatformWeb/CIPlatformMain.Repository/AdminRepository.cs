using CIPlatformMain.Entities.Data;
using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CIPlatformMain.Repository
{
    public class AdminRepository : IAdmin
    {
        private readonly CidatabaseContext _cidatabase;

        public AdminRepository()
        {
            _cidatabase = new CidatabaseContext();
        }


        //Admin Login Method
        public bool AdminLogin(Admin admin)
        {
            var Admin = _cidatabase.Admins.Where(a => a.Email.Equals(admin.Email) && a.Password.Equals(admin.Password)).FirstOrDefault();
            if (Admin != null)
            {
                
                return true;
            }
            else
            {
                return false;
            }

        }

        //Method For get data for admin pages
        public AdminData GetData()
        {
            AdminData Data = new AdminData();
            Data.Country = _cidatabase.Countries.ToList();
            Data.City = _cidatabase.Cities.ToList();
            Data.Skills = _cidatabase.Skills.ToList();
            Data.Users = _cidatabase.Users.Where(u => u.DeletedAt == null).ToList();
            Data.UserSkill = _cidatabase.UserSkills.ToList();
            Data.MissionApplications = _cidatabase.MissionApplications.Where(a => a.ApprovalStatus == "PENDING" && a.DeletedAt == null).ToList();
            Data.Missions = _cidatabase.Missions.Where(m=>m.DeletedAt==null).ToList();
            Data.Themes = _cidatabase.MissionThemes.Where(t => t.DeletedAt == null).ToList();
            Data.Stories = _cidatabase.Stories.ToList();
            Data.AllstoryMedia = _cidatabase.StoryMedia.ToList();
            Data.cmsPages = _cidatabase.CmsPages.Where(p => p.DeletedAt == null).ToList();
            Data.Banner = _cidatabase.Banners.Where(b=>b.DeletedAt==null).ToList();

            return Data;
        }



        public IEnumerable<City> getCities(long CountryId)
        {
            if (CountryId != null)
            {
                var cities = _cidatabase.Cities.Where(c=>c.CountryId==CountryId).ToList();
                return cities;
            }
            else
            {
                return Enumerable.Empty<City>();    
            }
        }
        //<------------------------------------------Methods For User------------------------------------------------->


        //To Delete User
        public bool DeleteUser(long UserId)
        {
            var User = _cidatabase.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            if (User != null)
            {
                User.DeletedAt = DateTime.Now;
                User.Status = false;
                _cidatabase.Update(User);
                _cidatabase.SaveChanges();

                var story = _cidatabase.Stories.Where(s => s.UserId == UserId).ToList();
                if (story.Count()>0)
                {
                    foreach(var item in story)
                    {
                        item.DeletedAt=DateTime.Now;
                        _cidatabase.Update(item); 
                        _cidatabase.SaveChanges();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }
 
        //To get perticular Userdata to edit
        public UserData GetUser(long UserId)
        {
            UserData User = new UserData();
            User.User = _cidatabase.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            User.Skills = _cidatabase.Skills.ToList();
            User.Country = _cidatabase.Countries.ToList();
            User.City = _cidatabase.Cities.ToList();

            User.userSkill = _cidatabase.UserSkills.Where(s => s.UserId == UserId).ToList();

            return User;
        }

        // To Check Same Email can't Registered 
        public int CheckEmailInDatabase(User _user)
        {
            User userstatus;

            if (_user.UserId != null)
            {
                 userstatus = _cidatabase.Users.Where(m => m.Email == _user.Email && m.UserId !=_user.UserId).FirstOrDefault();
            }
            else
            {
                userstatus = _cidatabase.Users.Where(m => m.Email == _user.Email).FirstOrDefault();
            }
            
            if (userstatus == null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

       
        //To Add NewUser
        public void AddUser(User user, IFormFile Avatar)
        {
            User User = new User();
            User.FirstName = user.FirstName;
            User.LastName = user.LastName;

            User.CityId = user.CityId;
            User.CountryId = user.CountryId;
            User.Title = user.Title;
            User.WhyIVolunteer = user.WhyIVolunteer;
            User.Department = user.Department;
            User.Email = user.Email;
            User.Password = user.Password;
            User.EmployeeId = user.EmployeeId;
            User.Manager = user.Manager;
            User.LinkedInUrl = user.LinkedInUrl;
            User.Avability = user.Avability;
            User.PhoneNumber = user.PhoneNumber;

            //To User Image
            if (Avatar != null)
            {
                FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(Avatar.FileName)), FileMode.Create);

                Avatar.CopyToAsync(FileStream);
                var ImageURL = "\\images\\" + Path.GetFileName(Avatar.FileName);

                User.Avatar = ImageURL;

                FileStream.Close();

            }
            _cidatabase.Users.Add(User);
            _cidatabase.SaveChanges();

        }

       
        //To Edit User
        public void EditUser(User User, long UserId, IFormFile Avatar)
        {
            User user = _cidatabase.Users.Where(x => x.UserId == UserId).FirstOrDefault();

            if (user != null)
            {


                user.FirstName = User.FirstName;
                user.LastName = User.LastName;
                user.CityId = User.CityId;
                user.CountryId = User.CountryId;
                user.ProfileText = User.ProfileText;
                user.Email = User.Email;
                user.Password = User.Password;
                user.EmployeeId = User.EmployeeId;
                user.Department = User.Department;
                user.UpdatedAt = DateTime.Now;

                user.Status = User.Status;
                if (Avatar != null)
                {
                    FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(Avatar.FileName)), FileMode.Create);

                    Avatar.CopyToAsync(FileStream);
                    var ImageURL = "\\images\\" + Path.GetFileName(Avatar.FileName);

                    user.Avatar = ImageURL;

                    FileStream.Close();

                }
                _cidatabase.Update(user);
                _cidatabase.SaveChanges();
            }

        }

        
        
        //<------------------------------------------Methods For Mission Start------------------------------------------------->

        
        
        //To Get Data For Add Mission Page
        public AddMission getMissionData()
        {
            AddMission addMission = new AddMission();
            addMission.Missions = _cidatabase.Missions.ToList();
            addMission.Countries = _cidatabase.Countries.ToList();
            addMission.Cities = _cidatabase.Cities.ToList();
            addMission.Skill = _cidatabase.Skills.ToList();
            addMission.Theme = _cidatabase.MissionThemes.ToList();
            addMission.MissionMediums = _cidatabase.MissionMedia.ToList();
            addMission.MissionsDocuments = _cidatabase.MissionDocuments.ToList();
            addMission.MissionSkills = _cidatabase.MissionSkills.ToList();
            addMission.GoalMissions = _cidatabase.GoalMissions.ToList();



            return addMission;
        }


        //To Get MissionMedia Data
        public IEnumerable<MissionMedium> GetMissionMedia(long MissionId)
        {
            
                var MissionMedia=_cidatabase.MissionMedia.Where(m=>m.MissionId==MissionId).ToList();
                return MissionMedia;
           
        }

        
        //To Add New Mission
        public bool AddMission(Mission mission, List<IFormFile> Images, IFormFile DefaultImage, List<IFormFile> Documents, String MissionVideoURL, List<int> SkillList,GoalMission goalMission)
        {
            if (mission != null)
            {
                Mission newMission = new Mission();
                newMission.Title = mission.Title;
                newMission.Description = mission.Description;
                newMission.ShortDescription = mission.ShortDescription;
                newMission.OrganizationName = mission.OrganizationName;
                newMission.OrganizationDetail = mission.OrganizationDetail;
                newMission.MissionType = mission.MissionType;
                newMission.StartDate = mission.StartDate;
                newMission.EndDate = mission.EndDate;
               
                newMission.CityId = mission.CityId;
                newMission.CountryId = mission.CountryId;
                newMission.SeatsLeft = mission.TotalSeats;
                newMission.Availability = mission.Availability;
                newMission.ThemeId = mission.ThemeId;

                if (goalMission != null && mission.MissionType=="Goal")
                {
                    GoalMission newGoalMission = new GoalMission();
                    newGoalMission.GoalValue = goalMission.GoalValue;
                    newGoalMission.GoalObjectiveText = goalMission.GoalObjectiveText;   

                    newMission.GoalMissions.Add(newGoalMission);
                }
                else
                {
                    newMission.Deadline = mission.Deadline;
                    newMission.TotalSeats = mission.TotalSeats;
                }

                if (DefaultImage != null)
                {
                    FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(DefaultImage.FileName)), FileMode.Create);

                    DefaultImage.CopyToAsync(FileStream);
                    var ImageURL = "\\images\\" + Path.GetFileName(DefaultImage.FileName);
                    newMission.MissionImg = ImageURL;

                    FileStream.Close();
                }

                if (Images.Count != 0)
                {
                    foreach (var file in Images)
                    {

                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(file.FileName)), FileMode.Create);

                        file.CopyToAsync(FileStream);
                        var ImageURL = "\\images\\" + Path.GetFileName(file.FileName);

                        MissionMedium missionMedium = new MissionMedium();
                        missionMedium.MediaPath = ImageURL;
                        missionMedium.MediaName = file.FileName;
                        missionMedium.MediaType = "IMG";
                        newMission.MissionMedia.Add(missionMedium);
                        FileStream.Close();
                    }
                }
                //AddMissionVideo URL
                if (MissionVideoURL != null)
                {

                    string[] URLS = MissionVideoURL.Split(',');

                    foreach(var url in URLS)
                    {
                        //To Check only youtube url should Enter
                        Match regexMatch = Regex.Match(url, "^[^v]+v=(.{11}).*",
                                    RegexOptions.IgnoreCase);

                        if (regexMatch.Success)
                        {

                            MissionMedium missionMedium = new MissionMedium();
                            missionMedium.MediaPath = url;
                            missionMedium.MediaName = mission.Title;
                            missionMedium.MediaType = "URL";

                            newMission.MissionMedia.Add(missionMedium);
                        }
                    }
                   
                }
                //AddMissionSkill
                if (SkillList.Count() > 0)
                {
                    foreach (var item in SkillList)
                    {
                        MissionSkill skill = new MissionSkill();
                        skill.SkillId = item;
                        newMission.MissionSkills.Add(skill);
                    }

                }
                if (Documents.Count != 0)
                {
                    foreach (var file in Documents)
                    {

                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(file.FileName)), FileMode.Create);

                        file.CopyToAsync(FileStream);
                        var DocsURL = "\\Uploads\\" + Path.GetFileName(file.FileName);

                        MissionDocument missionDocument = new MissionDocument();
                        missionDocument.DocumentName= file.FileName;
                        missionDocument.DocumentPath = DocsURL;
                        missionDocument.DocumentType = "DOC";
                        newMission.MissionDocuments.Add(missionDocument);
                        FileStream.Close();
                      
                    }
                }
                _cidatabase.Add(newMission);
                _cidatabase.SaveChanges();

            }
            return true;
        }


        //To Edit Existance Mission
        public bool EditMission(Mission mission, List<IFormFile> Images, IFormFile DefaultImage, List<IFormFile> Documents, string MissionVideoURL, List<int> SkillList, string preloadedImages,GoalMission goalMission)
        {
            Mission newMission = _cidatabase.Missions.Where(m => m.MissionId == mission.MissionId).FirstOrDefault();
            if (newMission!= null)
            {
                
                newMission.Title = mission.Title;
                newMission.Description = mission.Description;
                newMission.ShortDescription = mission.ShortDescription;
                newMission.OrganizationName = mission.OrganizationName;
                newMission.OrganizationDetail = mission.OrganizationDetail;
                newMission.MissionType = mission.MissionType;
                newMission.StartDate = mission.StartDate;
                newMission.EndDate = mission.EndDate;
               
                newMission.CityId = mission.CityId;
                newMission.CountryId = mission.CountryId;
                
                newMission.Availability = mission.Availability;
                newMission.ThemeId = mission.ThemeId;
               
                
                if (goalMission != null && mission.MissionType == "Goal")
                {
                    GoalMission newGoalMission = new GoalMission();
                    newGoalMission.GoalValue = goalMission.GoalValue;
                    newGoalMission.GoalObjectiveText = goalMission.GoalObjectiveText;

                    newMission.GoalMissions.Add(newGoalMission);
                }
                else
                {
                    newMission.Deadline = mission.Deadline;
                    newMission.TotalSeats = mission.TotalSeats;
                }

                if (DefaultImage != null)
                {
                    FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(DefaultImage.FileName)), FileMode.Create);

                    DefaultImage.CopyToAsync(FileStream);
                    var ImageURL = "\\images\\" + Path.GetFileName(DefaultImage.FileName);
                    newMission.MissionImg = ImageURL;

                    FileStream.Close();
                }
                var oldmedia = _cidatabase.MissionMedia.Where(m => m.MissionId == mission.MissionId && m.MediaType == "IMG").ToList();
                if (oldmedia.Count() > 0)
                {
                    foreach (var item in oldmedia)
                    {
                        _cidatabase.Remove(item);
                        _cidatabase.SaveChanges();
                    }
                }
                if (preloadedImages != null)
                {
                   
                        string[] PreloadedImages = preloadedImages.Split(',');
                    
                    foreach (var item in PreloadedImages)
                    {
                        MissionMedium missionMedium = new MissionMedium();

                        missionMedium.MediaPath = item;
                        missionMedium.MediaName = mission.Title;
                        missionMedium.MediaType = "IMG";
                        newMission.MissionMedia.Add(missionMedium);
                    }
                }

                if (Images.Count != 0)
                {
                   
                    foreach (var file in Images)
                    {
                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(file.FileName)), FileMode.Create);

                        file.CopyToAsync(FileStream);
                        var ImageURL = "\\images\\" + Path.GetFileName(file.FileName);

                        MissionMedium missionMedium = new MissionMedium();
                       
                        missionMedium.MediaPath = ImageURL;
                        missionMedium.MediaName = mission.Title;
                        missionMedium.MediaType = "IMG";
                        newMission.MissionMedia.Add(missionMedium);
                        FileStream.Close();
                    }
                }
                if (MissionVideoURL != null)
                {
                    var oldmediaurl = _cidatabase.MissionMedia.Where(m => m.MissionId == mission.MissionId && m.MediaType == "URL").ToList();
                    if (oldmediaurl.Count() > 0)
                    {
                        foreach (var item in oldmediaurl)
                        {
                            _cidatabase.Remove(item);
                            _cidatabase.SaveChanges();
                        }
                    }



                    string[] URLS = MissionVideoURL.Split(',');

                    foreach (var url in URLS)
                    {
                        //To Check only youtube url should Enter
                        Match regexMatch = Regex.Match(url, "^[^v]+v=(.{11}).*",
                                    RegexOptions.IgnoreCase);

                        if (regexMatch.Success)
                        {

                            MissionMedium missionMedium = new MissionMedium();
                            missionMedium.MediaPath = url;
                            missionMedium.MediaName = mission.Title;
                            missionMedium.MediaType = "URL";

                            newMission.MissionMedia.Add(missionMedium);
                        }
                    }

                }
               
                if (SkillList.Count() > 0)
                {
                    var oldskills = _cidatabase.MissionSkills.Where(s => s.MissionId == mission.MissionId).ToList();
                    if (oldskills.Count() > 0)
                    {
                        foreach (var item in oldskills)
                        {
                            _cidatabase.Remove(item);
                            _cidatabase.SaveChanges();

                        }
                    }

                    foreach (var item in SkillList)
                    {
                        MissionSkill skill = new MissionSkill();
                        skill.SkillId = item;
                        newMission.MissionSkills.Add(skill);
                    }

                }
                if (Documents.Count != 0)
                {
                    var oldmediadoc = _cidatabase.MissionDocuments.Where(m => m.MissionId == mission.MissionId).ToList();
                    if (oldmediadoc.Count() > 0)
                    {
                        foreach (var item in oldmediadoc)
                        {
                            _cidatabase.Remove(item);
                            _cidatabase.SaveChanges();
                        }
                    }

                    foreach (var file in Documents)
                    {

                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(file.FileName)), FileMode.Create);

                        file.CopyToAsync(FileStream);
                        var DocsURL = "\\Uploads\\" + Path.GetFileName(file.FileName);

                        MissionDocument missionDocument = new MissionDocument();
                        missionDocument.DocumentName = file.FileName;
                        missionDocument.DocumentPath = DocsURL;
                        missionDocument.DocumentType = "DOC";
                        newMission.MissionDocuments.Add(missionDocument);
                        FileStream.Close();

                       
                    }
                }
                _cidatabase.Update(newMission);
                _cidatabase.SaveChanges();

            }
            return true;
        }


        //To Delete Mission
        public bool DeleteMission(long MissionId)
        {
            var mission = _cidatabase.Missions.Where(m => m.MissionId == MissionId).FirstOrDefault();
            if (mission != null)
            {
                mission.DeletedAt = DateTime.Now;
                _cidatabase.Update(mission);
                

                var missionapplication = _cidatabase.MissionApplications.Where(m => m.MissionId == MissionId).ToList();
                foreach (var item in missionapplication)
                {
                    item.DeletedAt = DateTime.Now;
                    _cidatabase.Update(item);
                    
                }
                _cidatabase.SaveChanges();

                
                return true;
            }
            else
            {
                return false;
            }
        }

        //<------------------------------------------Methods For Mission End------------------------------------------------->

       
        //<------------------------------------------Methods For Application------------------------------------------------->



        //To Approve Mission Application Of User
        public bool ApproveMissionApplication(long MissionId, long UserId)
        {
            MissionApplication application = _cidatabase.MissionApplications.Where(a => a.MissionId == MissionId && a.UserId == UserId).FirstOrDefault();
            if (application != null)
            {
                application.ApprovalStatus = "APPROVED";
                _cidatabase.Update(application);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }
        }
       
        
        
        //Decline Mission Application
        public bool DeclineMissionApplication(long MissionId, long UserId)
        {
            MissionApplication application = _cidatabase.MissionApplications.Where(a => a.MissionId == MissionId && a.UserId == UserId).FirstOrDefault();
            if (application != null)
            {
                application.ApprovalStatus = "DECLINE";
                application.DeletedAt = DateTime.Now;
                _cidatabase.Update(application);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }
        }



        //<------------------------------------------Methods For Story------------------------------------------------->

      
        
        //To Decline Story Publish Request
        public bool StoryDecline(long StoryId)
        {
            var story = _cidatabase.Stories.Where(s => s.StoryId == StoryId).FirstOrDefault();
            if (story != null)
            {
                story.Status = "DECLINED";
                _cidatabase.Update(story);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //To Approve Story Publish Request
        public bool StoryApprove(long StoryId)
        {
            var story = _cidatabase.Stories.Where(s => s.StoryId == StoryId).FirstOrDefault();
            if (story != null)
            {
                story.Status = "PUBLISHED";
                story.PublishedAt = DateTime.Now;

                _cidatabase.Update(story);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
       
        
        //To Delete Story
        public bool StoryDelete(long StoryId)
        {
            var story = _cidatabase.Stories.Where(s => s.StoryId == StoryId).FirstOrDefault();
            if (story != null)
            {
                story.DeletedAt = DateTime.Now;
                story.Status = "DECLINED";

                _cidatabase.Update(story);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }



        //<------------------------------------------Methods For Skill------------------------------------------------->
        //to Add New SKill
       public  bool AddSkill(string SkillName, int SkillStatus)
        {
            if (SkillName != null && SkillStatus != null)
            {
                Skill skill =new Skill();
                skill.SkillName = SkillName;
                if (SkillStatus == 0)
                {
                    skill.Status = 0;
                }
                else
                {
                    skill.Status = 1;
                }
                _cidatabase.Add(skill);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //To Edit skills avalabel for Mission and User
        public bool EditSkill(long SkillId, string SkillName,int SkillStatus)
        {
            var skill = _cidatabase.Skills.Where(s => s.SkillId == SkillId).FirstOrDefault();
            if (skill != null)
            {
                skill.SkillName = SkillName;
                if (SkillStatus == 0)
                {
                    skill.Status = 0;
                }
                else
                {
                    skill.Status = 1;
                }
                skill.UpdatedAt = DateTime.Now;
                _cidatabase.Update(skill);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        //Delete Skill
        public bool DeleteSkill(long SkillId)
        {
            Skill skill = _cidatabase.Skills.Where(s => s.SkillId == SkillId).FirstOrDefault();
            if (skill != null)
            {
                skill.DeletedAt = DateTime.Now;
               
                _cidatabase.Update(skill);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //For Get Skill For Draft
        public Skill GetSkill(long SkillId)
        {
            Skill skill = _cidatabase.Skills.Where(s => s.SkillId == SkillId).FirstOrDefault();
            return skill;
        }




        //<------------------------------------------Methods For Theme------------------------------------------------->


       
        //For Get Theme For Draft
        public MissionTheme GetMissionTheme(long MissionThemeId)
        {
            MissionTheme missionTheme = _cidatabase.MissionThemes.Where(t => t.MissionThemeId == MissionThemeId).FirstOrDefault();
            return missionTheme;
        }
       
        
        // For Add Theme 
         public bool AddTheme(string ThemeName, int Status)
        {
            if(ThemeName!=null && Status!=0)
            {
                MissionTheme missionTheme = new MissionTheme();
                missionTheme.Title = ThemeName;
                if (Status == 0)
                {
                    missionTheme.Status = 0;
                }
                else
                {
                    missionTheme.Status = 1;
                }
               
                _cidatabase.Add(missionTheme);
                _cidatabase.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
       
        
        //For Edit Theme
        public bool EditTheme(string ThemeName, int Status, long ThemeId)
        {
            if (ThemeName != null && Status != null && ThemeId!=null)
            {
                MissionTheme missionTheme = _cidatabase.MissionThemes.Where(t => t.MissionThemeId == ThemeId).FirstOrDefault();
                missionTheme.Title = ThemeName;
                if (Status == 0)
                {
                    missionTheme.Status = 0;
                }
                else
                {
                    missionTheme.Status = 1;
                }

                _cidatabase.Update(missionTheme);
                _cidatabase.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        
        //For Delete Theme
        public bool DeleteTheme(long ThemeId)
        {
            MissionTheme theme = _cidatabase.MissionThemes.Where(t => t.MissionThemeId == ThemeId).FirstOrDefault();
            if (theme != null)
            {
                theme.DeletedAt = DateTime.Now;
              
                _cidatabase.Update(theme);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        
        
        //<------------------------------------------Methods For CMSPage------------------------------------------------->

        
        
        //For Adding New CMSPage
        public void AddCMSPage(CmsPage cmsPage)
        {
            _cidatabase.Add(cmsPage);
            _cidatabase.SaveChanges();
        }


        //For Edit CMSPage
        public void EditCMSPage(CmsPage cmsPage, long cmsId)
        {
            var CMSPage = _cidatabase.CmsPages.Where(c => c.CmsPageId == cmsId).FirstOrDefault();
            if (CMSPage != null)
            {
                CMSPage.Status = cmsPage.Status;
                CMSPage.Title = cmsPage.Title;
                CMSPage.Description = cmsPage.Description;
                CMSPage.Slug = cmsPage.Slug;
                _cidatabase.Update(CMSPage);
                _cidatabase.SaveChanges();
            }
        }


        //For Delete CMSPage
        public bool DeleteCMSPage(long CMSPageId)
        {
            CmsPage cmspage = _cidatabase.CmsPages.Where(p => p.CmsPageId == CMSPageId).FirstOrDefault();
            if (cmspage != null)
            {
                cmspage.DeletedAt = DateTime.Now;
                _cidatabase.Update(cmspage);
                _cidatabase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }


        }


        //To Get One Banner for edit
        public Banner GetBanner(long BannerId)
        {
                 Banner banner = _cidatabase.Banners.Where(b => b.BannerId == BannerId).FirstOrDefault();
                    return banner;
               
          

        }


        //For Delete Banner
        public bool DeleteBanner(long BannerId)
        {
            if (BannerId != null)
            {
                Banner banner=_cidatabase.Banners.Where(b=>b.BannerId==BannerId).FirstOrDefault();
                if (banner != null)
                {
                    banner.DeletedAt= DateTime.Now;
                    _cidatabase.Update(banner);
                    _cidatabase.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


        //To Add Banner
       public  bool AddBanner(Banner banner, IFormFile BannerImage)
        {
           
                if (banner != null)
                {
                    Banner Banner = new Banner();
                    Banner.Text = banner.Text;
                    Banner.SortOrder = banner.SortOrder;
                    if (BannerImage != null)
                    {

                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(BannerImage.FileName)), FileMode.Create);

                        BannerImage.CopyToAsync(FileStream);
                        var ImageURL = "\\Uploads\\" + Path.GetFileName(BannerImage.FileName);
                        Banner.Image = ImageURL;

                        FileStream.Close();
                    }
                    _cidatabase.Add(Banner);
                    _cidatabase.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            
        }


        //To Edit Banner
        public bool EditBanner(long BannerId, Banner banner, IFormFile BannerImage)
        {
            if (BannerId != null)
            {
                var Banner = _cidatabase.Banners.Where(b => b.BannerId== BannerId).FirstOrDefault();
                if (Banner != null)
                {
                    Banner.SortOrder = banner.SortOrder;
                    Banner.Text = banner.Text;
                    if (BannerImage != null)
                    {
                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(BannerImage.FileName)), FileMode.Create);

                        BannerImage.CopyToAsync(FileStream);
                        var ImageURL = "\\images\\" + Path.GetFileName(BannerImage.FileName);
                        Banner.Image= ImageURL;

                        FileStream.Close();
                    }
                  
                    _cidatabase.Update(Banner);
                    _cidatabase.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
