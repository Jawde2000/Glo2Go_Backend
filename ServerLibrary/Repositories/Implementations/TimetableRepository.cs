using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ServerLibrary.Repositories.Implementations
{
    public class TimetableRepository(Glo2GoDbContext dbContext) : ITimetable
    {
        public async Task<GeneralResponse> CreateTimetableAsync(CreateTimetableDTO timetable)
        {
            if (timetable == null)
            {
                return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            string tableID = await GetNextId();

            var newTimetable = new Timetable()
            {
                TimelineID = tableID,
                TimelineTitle = timetable.TimelineTitle,
                TimelineStartDate = timetable.TimelineStartDate,
                TimelineEndDate = timetable.TimelineEndDate,
                TravelerEmail = timetable.TravelerEmail,
                Traveler = timetable.Traveler,
            };

            var addTable = AddToDB(newTimetable);

            if (addTable != null) return new GeneralResponse(true, "Your timetable has been successfully created! Ready to explore your next adventure?");

            throw new NotImplementedException();
        }

        private async Task<string> GetNextId()
        {
            // Get the highest current numeric ID
            var tableIds = await dbContext.Timetables
            .Select(s => s.TimelineID.Substring(4)) // Skip the first 4 characters ("G2GS")
            .ToListAsync();

            var maxId = tableIds
                .Select(id => int.TryParse(id, out var numericId) ? numericId : (int?)null)
                .Max();

            // If there are no sites yet, start at 1, otherwise increment the max ID
            int nextNumericId = (maxId ?? 0) + 1;

            // Convert the numeric ID to a string, pad it with leading zeros to be 8 digits long,
            // and prepend "G2GS"
            string nextId = "G2GT" + nextNumericId.ToString("D8");

            return nextId;
        }

        private async Task<T> AddToDB<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}
