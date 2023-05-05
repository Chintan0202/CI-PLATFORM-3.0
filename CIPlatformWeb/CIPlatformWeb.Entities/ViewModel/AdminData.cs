using CIPlatformMain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class AdminData
    {
        public IEnumerable<City>? City { get; set; }

        public IEnumerable<Country>? Country { get; set; }

        public IEnumerable<Skill>? Skills { get; set; }

        public IEnumerable<UserSkill>? UserSkill { get; set; }

        public IEnumerable<User>? Users { get; set; }

        public User? User { get; set; }

        public IEnumerable<MissionApplication>? MissionApplications { get; set; }

        public IEnumerable<Mission>? Missions { get; set; }

        public IEnumerable<MissionTheme>? Themes { get; set; }

       public IEnumerable<Story>? Stories { get; set; }

        public Story? Story { get; set; }

        public IEnumerable<StoryMedium>? AllstoryMedia { get; set; }
        public IEnumerable<StoryMedium>? storyMedia { get; set; }

        public IEnumerable<CmsPage>? cmsPages { get; set; }

        public CmsPage? CmsPage { get; set; }  

        public IEnumerable<Banner> Banner { get; set; }

        public Banner bannerData { get; set; }

        public MissionTheme MissionTheme { get; set; }



    }
}
