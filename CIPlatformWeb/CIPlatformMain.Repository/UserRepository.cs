using CIPlatformMain.Entities.Data;
using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Repository
{
    public class UserRepository :IUser
    {
        private readonly CidatabaseContext  _cidatabase;

        public UserRepository() { 
        
            _cidatabase= new CidatabaseContext();

        } 

        public User Login(User user)
        {
            var User = _cidatabase.Users.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password) && a.DeletedAt == null && a.Status==true).FirstOrDefault();
            return User;
        }
        public User DeletedUser(User user)
        {
            var User = _cidatabase.Users.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();
            return User;
        }

        public User GetUser(long User_id)
        {
            var user=_cidatabase.Users.Where(u=>u.UserId == User_id).FirstOrDefault();
            return user;
        }

        public int CheckEmailInDatabase(User _user) {

            var userstatus = _cidatabase.Users.Where(m => m.Email == _user.Email).FirstOrDefault();
            if (userstatus==null)
            {
                return 1;
            }
            else
            {
                return 0;
            }



        }
        public void Register(User _user)
        {
            _user.CityId = 2;
            _user.CountryId = 1;
            _cidatabase.Users.Add(_user);
            _cidatabase.SaveChanges();
            
        }

        public UserData GetUserData(long User_id)
        {
            UserData data = new UserData();
            data.User=_cidatabase.Users.Where(u=>u.UserId == User_id).FirstOrDefault();
           
            data.Skills = _cidatabase.Skills.ToList();
            data.Country=_cidatabase.Countries.ToList();
            data.City=_cidatabase.Cities.ToList();
          
            
            data.userSkill=_cidatabase.UserSkills.Where(s=>s.UserId==User_id).ToList();



            return data;
        }

        public int UpdateUser(UserData _userdata, int cityid, int countryid, long User_id, IFormFile Avatar)
        {
            User user = _cidatabase.Users.FirstOrDefault(u => u.UserId == User_id);
            user.FirstName = _userdata.User.FirstName;
            user.LastName = _userdata.User.LastName;
            user.Title = _userdata.User.Title; 
            user.ProfileText = _userdata.User.ProfileText;
            user.Department = _userdata.User.Department;
            user.CountryId= countryid;
            user.CityId= cityid;
            user.EmployeeId=_userdata.User.EmployeeId;
            user.Manager = _userdata.User.Manager;
            user.WhyIVolunteer = _userdata.User.WhyIVolunteer;

            if (Avatar != null)
            {
                FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(Avatar.FileName)), FileMode.Create);

                Avatar.CopyToAsync(FileStream);
                var ImageURL = "\\images\\" + Path.GetFileName(Avatar.FileName);

                user.Avatar = ImageURL;

                FileStream.Close();

            }


            _cidatabase.Users.Update(user);

            _cidatabase.SaveChanges();

            return 1;
        }

        public int UpdatePassword(string NewPassword, string ConformPassword, string oldpassword,long User_id)
        {
            User user = _cidatabase.Users.FirstOrDefault(u => u.UserId == User_id);

            if (user.Password == oldpassword)
            {
               
                    user.Password = NewPassword;
                    _cidatabase.Update(user);
                    _cidatabase.SaveChanges();
                    return 1;
               
                    
            }
            else
            {
                return 2;
            }

            
        }

       public bool UpdateSkills(long[] Userskills, long User_id)
        {
            var userskills=_cidatabase.UserSkills.Where(u=>u.UserId== User_id).ToList();
           foreach (var skill in userskills)
            {
                        _cidatabase.Remove(skill);
                        _cidatabase.SaveChanges();
            }
           foreach(long skill_id in Userskills)
            {
                UserSkill userSkill = new UserSkill();
                userSkill.SkillId = skill_id;
                userSkill.UserId = User_id;

                _cidatabase.Add(userSkill);
                _cidatabase.SaveChanges();
            }

            return true;
        }
        
        public VolunteeringTimesheet GetVolunteeringTimesheet(long userid)
        {
            
            VolunteeringTimesheet timesheet = new VolunteeringTimesheet();
            timesheet.Timesheets = _cidatabase.Timesheets.Where(t => t.UserId == userid && t.Status== "APPROVED" &&t.DeletedAt==null).ToList();
            timesheet.MissionGoal = _cidatabase.GoalMissions.ToList();
            timesheet.Mission = _cidatabase.Missions.ToList();
            timesheet.MissionApplied=_cidatabase.MissionApplications.Where(a=>a.UserId == userid && a.ApprovalStatus== "APPROVED").ToList(); 
            return timesheet;
        }
        public Mission GetMission(long MissionId)
        {
            return _cidatabase.Missions.Where(m=>m.MissionId==MissionId).FirstOrDefault();
        }

        public bool AddGoalTimesheet(long MissionId, int Action, DateTime DateVolunteered, string Message, long User_id)
        {
            Timesheet newtimesheet= new Timesheet();
            newtimesheet.MissionId = MissionId;
            newtimesheet.Action = Action;
            newtimesheet.DateVolunteered = DateVolunteered;
            newtimesheet.UserId = User_id;
            newtimesheet.Notes=Message;

            _cidatabase.Add(newtimesheet);
            _cidatabase.SaveChanges();

            return true;
        }

        //To Edit Goal Time Sheet
        public bool EditGoalTimesheet(int Action, DateTime DateVolunteered, string Message, long TimesheetId)
        {
            Timesheet newtimesheet = _cidatabase.Timesheets.Where(t=>t.TimesheetId==TimesheetId).FirstOrDefault();
           
            newtimesheet.Action = Action;
            newtimesheet.DateVolunteered = DateVolunteered;
           
            newtimesheet.Notes = Message;

            _cidatabase.Update(newtimesheet);
            _cidatabase.SaveChanges();

            return true;
        }

        //To Add TimeTimesheet
        public bool AddTimeTimesheet(long MissionId, DateTime DateVolunteered, string Message, string Hours, string Minutes, long User_id)
        {
            string time = Hours + ":" + Minutes;
            
            Timesheet timesheet= new Timesheet();
            timesheet.MissionId = MissionId;
            timesheet.UserId= User_id;
            timesheet.Notes=Message;
            timesheet.Time = TimeOnly.Parse(time);
            timesheet.DateVolunteered= DateVolunteered;

            _cidatabase.Add(timesheet);
            _cidatabase.SaveChanges();
            return true;
        }
       
        //EditTimeSheet
        public bool EditTimeTimesheet( DateTime DateVolunteered, string Message, string Hours, string Minutes, long TimesheetId)
        {
            string time = Hours + ":" + Minutes;
            Timesheet timesheet = _cidatabase.Timesheets.Where(t=>t.TimesheetId== TimesheetId).FirstOrDefault();
           
            timesheet.Notes = Message;
            timesheet.Time = TimeOnly.Parse(time);
            timesheet.DateVolunteered = DateVolunteered;

            _cidatabase.Update(timesheet);
            _cidatabase.SaveChanges();
            return true; 
        }

        public bool DeleteTimeSheet(long TimesheetId)
        {
            var Timesheet =_cidatabase.Timesheets.Where(t=>t.TimesheetId==TimesheetId).FirstOrDefault();
            _cidatabase.Remove(Timesheet);
            _cidatabase.SaveChanges();

            return true;
        }

        public Timesheet GetTimesheetData(long TimesheetId)
        {
                Timesheet Data = _cidatabase.Timesheets.Where(t => t.TimesheetId == TimesheetId).FirstOrDefault();       
                return Data;
            
        }

        public IEnumerable<City> GetCities(int Country_id)
        {
            var cities=_cidatabase.Cities.Where(c=>c.CountryId==Country_id).ToList();
            return cities;
        }
    }
}





