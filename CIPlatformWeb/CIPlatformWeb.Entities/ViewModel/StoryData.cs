using CIPlatformMain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class StoryData
    {
        public IEnumerable<User> user { get; set; }

        public IEnumerable<Story> story { get; set; }

        public IEnumerable<StoryMedium> storyMedia { get; set; }

        public Story Story { get; set; }

        public IEnumerable<Mission> mission { get; set; }

        public IEnumerable<MissionTheme> Theme { get; set; }

        public IEnumerable<MissionApplication>? Applications { get; set; }

    }
}
