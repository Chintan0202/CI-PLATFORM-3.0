using CIPlatformMain.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Entities.ViewModel
{
    public class UserData
    {
       

        public IEnumerable<City>? City { get; set; }

        public IEnumerable<Country>? Country { get; set; }

        public IEnumerable<Skill>? Skills { get; set; }

        public User? User { get; set; }

        public IEnumerable<UserSkill>? userSkill { get; set; }

       
    }
}
