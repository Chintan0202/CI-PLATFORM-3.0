using CIPlatformMain.Entities.Data;
using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Repository.Interface;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;

namespace CIPlatformMain.Repository
{
    public class HomeRepository : IHome
    {
        private readonly CidatabaseContext _cidatabase;

        public HomeRepository()
        {
            _cidatabase = new CidatabaseContext();
        }


        //Return Mission Data For Landing Page
        public Missiondata GetMissions()
        {
            // Query to retrieve mission ratings...
            var avgRatings = _cidatabase.MissionRatings
                .GroupBy(r => r.MissionId)
                .Select(g => new VMMissionRating
                {
                    Id = g.Key,
                    MissionRating = g.Average(r => r.Rating)
                })
                .ToList();
            var missiondata = new Missiondata
            {
                Missions = _cidatabase.Missions.Where(m => m.DeletedAt == null).ToList(),
                Theme = _cidatabase.MissionThemes.ToList(),
                City = _cidatabase.Cities.ToList(),
                Country = _cidatabase.Countries.ToList(),
                GoalMissions = _cidatabase.GoalMissions.ToList(),
                Skills = _cidatabase.Skills.ToList(),
                FavMission = _cidatabase.FavoriteMissions.ToList(),
                Application = _cidatabase.MissionApplications.ToList(),
                User = _cidatabase.Users.ToList(),
                MissionSkills = _cidatabase.MissionSkills.ToList(),
                Timesheets = _cidatabase.Timesheets.ToList(),
                Ratings = avgRatings
            };
          


            return missiondata;
        }

       
        
        //Get Ratings
        public IEnumerable<MissionRating> GetRatings()
        {
            var rating = _cidatabase.MissionRatings.ToList();

            return rating;
        }


        //Get Mission Details For Volunteringpage
        public MissionDetails GetMissionDetails(int missionid,long userid)
        {
            var mission = _cidatabase.Missions.Where(m => m.MissionId == missionid).FirstOrDefault();


            var missiondetail = new MissionDetails
            {
                Mission = _cidatabase.Missions.Where(m => m.MissionId == missionid).ToList(),
                Theme = _cidatabase.MissionThemes.ToList(),
                City = _cidatabase.Cities.ToList(),
                Country = _cidatabase.Countries.ToList(),
                GoalMissions = _cidatabase.GoalMissions.Where(g => g.MissionId == missionid).FirstOrDefault(),
                Skills = _cidatabase.Skills.ToList(),
                FavMission = _cidatabase.FavoriteMissions.Where(g => g.MissionId == missionid && g.UserId == userid).FirstOrDefault(),
                Application = _cidatabase.MissionApplications.ToList(),
                User = _cidatabase.Users.ToList(),
                MissionSkills = _cidatabase.MissionSkills.Where(s => s.MissionId == missionid).ToList(),
                MissionMedia = _cidatabase.MissionMedia.Where(m => m.MissionId == missionid).ToList(),
               MissionDocuments = _cidatabase.MissionDocuments.Where(m => m.MissionId == missionid).ToList(),
                RelatedMission = _cidatabase.Missions.Where(m => m.CityId == mission.CityId || m.ThemeId == mission.ThemeId && m.MissionId != missionid).Take(3).ToList(),
                Timesheets = _cidatabase.Timesheets.ToList(),
                UserRating = _cidatabase.MissionRatings.Where(u => u.UserId == userid && u.MissionId==missionid).Select(r=>r.Rating).FirstOrDefault(),
                MissionComments = _cidatabase.Comments.Where(m=>m.MissionId==missionid).OrderByDescending(m=>m.CreatedAt).ToList(),
            };
            var MissionRatings = _cidatabase.MissionRatings.Where(r => r.MissionId == missionid).ToList();
            if (MissionRatings.Count() > 0)
            {
                double AvgRating = MissionRatings.Average(m => m.Rating);
                missiondetail.Rating = AvgRating;
                missiondetail.TotalRating = MissionRatings.Count();
            }
            else
            {
                missiondetail.Rating = 0;
                missiondetail.TotalRating = 0;
            }
            
            return missiondetail;
        }


        //Recommend Method For Mission Recommendation
        public void Recommend(int missionid, long from_userid, int to_userid)
        {
            var to_useremail = _cidatabase.Users.Where(u => u.UserId == to_userid).Select(u => u.Email).SingleOrDefault();
            if (to_useremail != null)
            {
                MissionInvite inviteobj = new MissionInvite();
                inviteobj.MissionId = missionid;
                inviteobj.ToUserId = to_userid;
                inviteobj.FromUserId = from_userid;

                _cidatabase.MissionInvites.Add(inviteobj);
                _cidatabase.SaveChanges();

                var mailbody = "<h1>Participate in this Mission</h1><br><h2><a href='" + "https://localhost:7037/Home/Volunteering_Mission_Page?missionid=" + missionid + "'>Wanna Join This Mission</a></h2>";
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("ciplatformmailsenderchintan@gmail.com"));

                email.To.Add(MailboxAddress.Parse(to_useremail));
                email.Subject = "Mission Recommendation";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = mailbody };

                using var smtp = new SmtpClient();
                //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //smtp.Authenticate("aksharpatel18092000@gmail.com", "gptnvpkcimqkuktp");
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("ciplatformmailsenderchintan@gmail.com", "pmjfsnfycrhnygrw");
                smtp.Send(email);
                smtp.Disconnect(true);


                



            }
        }
        
        
        
        //Method For MissionApplication
        public bool MissionApplication(long MissionId, long UserId)
        {
            if (MissionId != 0 && UserId!=0)
            {
                MissionApplication missionApplication = new MissionApplication();
            missionApplication.MissionId = MissionId;
            missionApplication.UserId = UserId;
                missionApplication.AppliedAt = DateTime.Now;
            
                Mission missionobj = _cidatabase.Missions.Where(m => m.MissionId == MissionId).FirstOrDefault();

                if (missionobj != null)
                {
                    missionobj.SeatsLeft = missionobj.SeatsLeft - 1;

                    _cidatabase.Update(missionobj);
                    _cidatabase.SaveChanges();
                }
                _cidatabase.Add(missionApplication);
                _cidatabase.SaveChanges(true);

                return true;
            }
            else
            {
                return false;
            }

           
        }


        //To Give Rating
        public bool GiveRating(int RatingValue,long MissionId, long UserId)
        {
            if (RatingValue != null && MissionId!=null&&UserId!=null)
            {
                var ratingstatus = _cidatabase.MissionRatings.Where(r => r.MissionId == MissionId && r.UserId == UserId).FirstOrDefault();
                if (ratingstatus == null)
                {
                    if (RatingValue > 0)
                    {
                        var ratingobj = new MissionRating();
                        ratingobj.MissionId = MissionId;
                        ratingobj.UserId = UserId;
                        ratingobj.Rating = RatingValue;
                        _cidatabase.MissionRatings.Add(ratingobj);
                        _cidatabase.SaveChanges();
                    }
                }
                if (ratingstatus != null)
                {
                    if (RatingValue > 0)
                    {
                        var mission_rating = _cidatabase.MissionRatings.Where(r => r.MissionId == MissionId && r.UserId == UserId).FirstOrDefault();
                        mission_rating.Rating = RatingValue;
                        _cidatabase.Update(mission_rating);
                        _cidatabase.SaveChanges();
                    }

                }
                return true;
            }
            else
            {
                return false;
            }
        }


        //Method For Contact Us
        public bool Contact_us(long user_id, string Subject, string Message)
        {
            ContactU contactU = new ContactU();
            contactU.UserId = user_id;
            contactU.Subject = Subject;
            contactU.Message = Message;

            _cidatabase.Add(contactU);
            _cidatabase.SaveChanges();

            return true;
        }

        public IEnumerable<Banner> GetBanners()
        {
            var banners = _cidatabase.Banners.Where(b=>b.DeletedAt==null).OrderBy(b=>b.SortOrder).ToList();
            return banners;
        }
    }
}
