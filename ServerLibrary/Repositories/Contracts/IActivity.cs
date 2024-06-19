using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IActivity
    {
        public Task<GeneralResponse> AddActivityAsync(CreateActivityDTO activity);
        public Task<GeneralResponse> UpdateActivityAsync(ActivityDTO activity);
        public Task<GeneralResponse> DeleteActivityAsync(string activityId);
        public Task<List<ActivityDTO>> GetActivityAsync(string timetableID);
    }
}
