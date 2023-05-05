using CIPlatformMain.Entities.Models;
using CIPlatformMain.Entities.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatformMain.Repository.Interface
{
    public interface IStory
    {
        StoryData GetStoryData();

        bool AddStory(long UserId,long MissionId, string StoryDescription, DateTime StoryDate, string StoryTitle, List<IFormFile> StoryImages, string StoryVideoURL,string submit);

        bool EditStory(long StoryId,long UserId, long MissionId, string StoryDescription, DateTime StoryDate, string StoryTitle, List<IFormFile> StoryImages, string StoryVideoURl, List<String> SavedImages,string submit);
    }
}
