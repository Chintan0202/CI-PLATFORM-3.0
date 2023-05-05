using CIPlatformMain.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class AddMission
    {
        public Mission? Mission { get; set; }

        public IEnumerable<Mission>? Missions { get; set; }

        public IEnumerable<Country>? Countries { get; set; }

        public IEnumerable<City>? Cities { get; set; }

        public IEnumerable<MissionTheme>? Theme { get; set; }

        public IEnumerable<Skill>? Skill { get; set; }  

        public IEnumerable<MissionDocument> MissionsDocuments { get; set; } 

        public IEnumerable<MissionMedium> MissionMediums { get; set; }

        public IEnumerable<MissionSkill> MissionSkills { get; set; }

        public MissionSkill MissionSkill { get; set; }

        public List<IFormFile>? Documents { get; set; }

        public IEnumerable<GoalMission> GoalMissions { get; set; }

        public GoalMission GoalMission { get; set; }


    }
}
