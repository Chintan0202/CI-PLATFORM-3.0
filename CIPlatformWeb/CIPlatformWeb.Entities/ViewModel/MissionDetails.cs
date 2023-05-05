using CIPlatformMain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class MissionDetails
    {
        public IEnumerable<Mission>? Mission { get; set; }

        public IEnumerable<MissionTheme>? Theme { get; set; }

        public IEnumerable<City>? City { get; set; }

        public IEnumerable<Country>? Country { get; set; }

        public GoalMission? GoalMissions { get; set; }

        public IEnumerable<Skill>? Skills { get; set; }

        public FavoriteMission? FavMission { get; set; }

        public IEnumerable<MissionSkill>? MissionSkills { get; set; }

        public IEnumerable<User>? User { get; set; }

        public IEnumerable<MissionApplication>? Application { get; set; }

        public IEnumerable<MissionMedium>? MissionMedia { get; set; }

        public IEnumerable<Mission>? RelatedMission { get; set; }

        public IEnumerable<Timesheet> Timesheets { get; set; }

        public double Rating { get; set; }

        public double TotalRating { get; set; }

        public double UserRating { get; set; }

        public IEnumerable<Comment>? MissionComments { get; set; }

        public IEnumerable<MissionDocument>? MissionDocuments { get; set; }

    }
}
