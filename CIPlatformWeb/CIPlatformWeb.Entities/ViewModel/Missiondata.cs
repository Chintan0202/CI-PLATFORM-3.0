using CIPlatformMain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class Missiondata
    {
        public  IEnumerable<Mission>? Missions { get; set; }

        public IEnumerable<MissionTheme>? Theme { get; set; }

        public IEnumerable<City>? City { get; set; }

        public IEnumerable<Country>? Country  { get; set; }

        public IEnumerable<GoalMission>? GoalMissions { get; set; }

        public IEnumerable<Skill>? Skills { get; set; }

        public IEnumerable<FavoriteMission>? FavMission { get; set; }

        public IEnumerable<MissionSkill>? MissionSkills { get; set; }

        public IEnumerable<User>? User { get; set; } 

        public IEnumerable<MissionApplication>? Application { get; set; }

        public IEnumerable<Timesheet>? Timesheets { get; set; }

       public IEnumerable<VMMissionRating> Ratings { get; set; }

    }
}
