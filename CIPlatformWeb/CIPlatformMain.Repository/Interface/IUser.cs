using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Repository.Interface
{
    public interface IUser
    {

        User Login(User user);

        User DeletedUser(User user);

         void Register(User _user);
         int CheckEmailInDatabase(User _user);

        User GetUser(long User_id);
       
        UserData GetUserData(long User_id);

        int UpdateUser(UserData _userdata, int cityid, int countryid,long User_id, IFormFile Avatar);

        int UpdatePassword(string NewPassword, string OldPassword, string oldpassword,long User_id);

        bool UpdateSkills(long[] Userskills, long User_id);

        VolunteeringTimesheet GetVolunteeringTimesheet(long User_id);

        bool AddGoalTimesheet(long MissionId, int Action, DateTime DateVolunteered, string Message, long User_id);

        bool EditGoalTimesheet(int Action, DateTime DateVolunteered, string Message, long TimesheetId);

        bool AddTimeTimesheet(long MissionId, DateTime DateVolunteered, string Message, string Hours, string Minutes, long User_id);

        bool EditTimeTimesheet(DateTime DateVolunteered, string Message, string Hours, string Minutes, long TimesheetId);

        bool DeleteTimeSheet(long TimesheetId);

        Mission GetMission(long MissionId);

        Timesheet GetTimesheetData(long TimesheetId);

        IEnumerable<City> GetCities(int Country_id);
    }
}
