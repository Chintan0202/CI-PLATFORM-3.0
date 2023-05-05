using CIPlatformMain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class VolunteeringTimesheet
    {
        public IEnumerable<Mission>? Mission { get; set; }

        public IEnumerable<GoalMission>? MissionGoal { get; set; }

        public IEnumerable<Timesheet>? Timesheets { get; set; }

        public IEnumerable<MissionApplication>? MissionApplied { get; set; }

        public Timesheet? Timesheet { get; set; }

    }
}
