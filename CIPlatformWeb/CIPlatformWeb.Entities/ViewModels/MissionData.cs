using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModels
{
    public class MissionData
    {
        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionTheme { get; set; } = null!;

        public string CountryName { get; set; } = null!;

        public string CityName { get; set; } = null!;
    }
}
