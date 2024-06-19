using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Implementations
{
    public class ActivityRepository(Glo2GoDbContext dbContext) : IActivity
    {
        public async Task<GeneralResponse> AddActivityAsync(CreateActivityDTO activity)
        {
            if (activity == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            // Generate the SiteID
            string activityId = await GetNextId(); // This method would get the next ID from your database

            // Prepare the new site
            var newActivity = new Activity()
            {
                ActivityID = activityId,
                ActivityTitle = activity.ActivityTitle,
                ActivityType = activity.ActivityType,
                ActivityStart = activity.ActivityStartTime,
                ActivityEnd = activity.ActivityEndTime,
                ActivityRegion = activity.ActivityRegion,
                ActivityDescription = activity.ActivityDescription,
                TimetableID = activity.TimelineID,
            };

            var addAcitivityTask = AddToDB(newActivity);

            if (addAcitivityTask != null) return new GeneralResponse(true, "Congratulations! Your event has been successfully created.");

            throw new NotImplementedException();
        }

        public async Task<GeneralResponse> UpdateActivityAsync(ActivityDTO activity)
        {
            if (activity == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var existingActivity = await dbContext.Activities.FirstOrDefaultAsync(a => a.ActivityID == activity.ActivityID);

            if (existingActivity == null) return new GeneralResponse(false, "The specified activity could not be found.");

            existingActivity.ActivityTitle = activity.ActivityTitle;
            existingActivity.ActivityType = activity.ActivityType;
            existingActivity.ActivityStart = activity.ActivityStartTime;
            existingActivity.ActivityEnd = activity.ActivityEndTime;
            existingActivity.ActivityRegion = activity.ActivityRegion;
            existingActivity.ActivityDescription = activity.ActivityDescription;
            existingActivity.TimetableID = activity.TimelineID;

            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "The activity has been successfully updated.");
        }

        public async Task<GeneralResponse> DeleteActivityAsync(string activityId)
        {
            if (string.IsNullOrEmpty(activityId)) return new GeneralResponse(false, "The activity ID must be provided.");

            var activity = await dbContext.Activities.FirstOrDefaultAsync(a => a.ActivityID == activityId);

            if (activity == null) return new GeneralResponse(false, "The specified activity could not be found.");

            dbContext.Activities.Remove(activity);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "The activity has been successfully deleted.");
        }

        public async Task<List<ActivityDTO>> GetActivityAsync(string timetableID)
        {
            if (string.IsNullOrEmpty(timetableID)) throw new ArgumentException("The timetable ID must be provided.");

            var activities = await dbContext.Activities
                .Where(a => a.TimetableID == timetableID)
                .ToListAsync();

            if (activities == null || !activities.Any()) return new List<ActivityDTO>();

            var activityDTOs = activities.Select(a => new ActivityDTO
            {
                ActivityID = a.ActivityID,
                ActivityTitle = a.ActivityTitle,
                ActivityType = a.ActivityType,
                ActivityStartTime = a.ActivityStart,
                ActivityEndTime = a.ActivityEnd,
                ActivityRegion = a.ActivityRegion,
                ActivityDescription = a.ActivityDescription,
                TimelineID = a.TimetableID,
            }).ToList();

            return activityDTOs;
        }

        private async Task<T> AddToDB<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        private async Task<string> GetNextId()
        {
            // Get the highest current numeric ID
            var activityIds = await dbContext.Activities
            .Select(s => s.ActivityID.Substring(4)) // Skip the first 4 characters ("G2GS")
            .ToListAsync();

            var maxId = activityIds
                .Select(id => int.TryParse(id, out var numericId) ? numericId : (int?)null)
                .Max();

            // If there are no sites yet, start at 1, otherwise increment the max ID
            int nextNumericId = (maxId ?? 0) + 1;

            // Convert the numeric ID to a string, pad it with leading zeros to be 8 digits long,
            // and prepend "G2GS"
            string nextId = "G2GE" + nextNumericId.ToString("D8");

            return nextId;
        }
    }
}
