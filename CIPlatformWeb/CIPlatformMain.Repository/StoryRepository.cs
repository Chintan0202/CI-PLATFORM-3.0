using CIPlatformMain.Entities.Data;
using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using CIPlatformMain.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CIPlatformMain.Repository
{
    public class StoryRepository : IStory
    {
        private readonly CidatabaseContext _cidatabase;

        public StoryRepository()
        {
            _cidatabase = new CidatabaseContext();
        }

        public StoryData GetStoryData()
        {
            var storydata = new StoryData
            {
                story = _cidatabase.Stories.Where(s=>s.DeletedAt==null).ToList(),
                storyMedia = _cidatabase.StoryMedia.ToList(),
                user = _cidatabase.Users.ToList(),
                mission=_cidatabase.Missions.ToList(),
                Theme=_cidatabase.MissionThemes.ToList(),
                Applications=_cidatabase.MissionApplications.ToList()

            };

            return storydata;
        }

        public bool AddStory(long UserId,long MissionId, string StoryDescription, DateTime StoryDate, string StoryTitle, List<IFormFile> StoryImages,string StoryVideoURL,string submit)
        {
            if (MissionId != null && StoryDate != null && UserId!=null)
            {

                Story newStory = new Story();
                
                newStory.UserId = UserId;
                newStory.MissionId = MissionId;
                newStory.PublishedAt = StoryDate;
                newStory.Title = StoryTitle;
               
                newStory.Description = StoryDescription;
                if (submit == "submit")
                {
                    newStory.Status = "PENDING";
                }
                else
                {
                    newStory.Status = "DRAFT";
                }
                if (StoryImages.Count() > 0)
                {
                    foreach (var file in StoryImages)
                    {

                        FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(file.FileName)), FileMode.Create);

                        file.CopyToAsync(FileStream);
                        var ImageURL = "\\images\\" + Path.GetFileName(file.FileName);
                        StoryMedium medias = new StoryMedium();

                       
                        medias.Path = ImageURL;
                        medias.Type = "IMG";

                        newStory.StoryMedia.Add(medias);
                        FileStream.Close();
                    }
                }
                if (StoryVideoURL != null)
                {
                    Match regexMatch = Regex.Match(StoryVideoURL, "^[^v]+v=(.{11}).*",
                              RegexOptions.IgnoreCase);

                    if (regexMatch.Success)
                    {

                        StoryMedium newStoryMedia = new StoryMedium();


                        newStoryMedia.Path = StoryVideoURL;
                        newStoryMedia.Type = "URL";

                        newStory.StoryMedia.Add(newStoryMedia);
                    }
                   
                }
                _cidatabase.Add(newStory);
                _cidatabase.SaveChanges();
            }
            return true;
        }
        public bool EditStory(long StoryId,long UserId, long MissionId, string StoryDescription, DateTime StoryDate, string StoryTitle, List<IFormFile> StoryImages, string StoryVideoURL,List<string> Preloaded,string submit)
        {
            if (MissionId != null && StoryDate != null && UserId != null && StoryId!=null)
            {

                Story newStory = _cidatabase.Stories.Where(s => s.StoryId == StoryId).FirstOrDefault();
                if (newStory != null)
                { 
                    newStory.UserId = UserId;
                    newStory.MissionId = MissionId;
                    newStory.PublishedAt = StoryDate;
                    newStory.Title = StoryTitle;
                   
                    newStory.Description = StoryDescription;

                    if (submit == "submit")
                    {
                        newStory.Status = "PENDING";
                    }
                    else
                    {
                        newStory.Status = "DRAFT";
                    }
                    var oldmedia = _cidatabase.StoryMedia.Where(m => m.StoryId == StoryId).ToList();
                    foreach (var item in oldmedia)
                    {
                        _cidatabase.Remove(item);
                        _cidatabase.SaveChanges();
                    }
                    if (StoryImages.Count() > 0)
                    {   
                        foreach (var file in StoryImages)
                        {

                            FileStream FileStream = new(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", Path.GetFileName(file.FileName)), FileMode.Create);

                            file.CopyToAsync(FileStream);
                            var ImageURL = "\\images\\" + Path.GetFileName(file.FileName);
                            StoryMedium medias = new StoryMedium();


                            medias.Path = ImageURL;
                            medias.Type = "IMG";

                            newStory.StoryMedia.Add(medias);
                            FileStream.Close();
                        }
                    }
                    
                        if (Preloaded.Count() > 0)
                        {
                            foreach (var item in Preloaded)
                            {
                                StoryMedium medias = new StoryMedium();
                                medias.Path = item;
                                medias.Type = "IMG";
                                newStory.StoryMedia.Add(medias);
                            }
                        }


                    if (StoryVideoURL != null)
                    {

                        Match regexMatch = Regex.Match(StoryVideoURL, "^[^v]+v=(.{11}).*",
                                  RegexOptions.IgnoreCase);

                        if (regexMatch.Success)
                        {

                            StoryMedium newStoryMedia = new StoryMedium();


                            newStoryMedia.Path = StoryVideoURL;
                            newStoryMedia.Type = "URL";

                            newStory.StoryMedia.Add(newStoryMedia);
                        }

                        
                    }
                    _cidatabase.Update(newStory);
                    _cidatabase.SaveChanges();
                }
            }
            return true;
        }

    }
}
