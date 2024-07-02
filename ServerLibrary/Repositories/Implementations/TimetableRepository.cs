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
                Country = timetable.Country,
                Region = timetable.Region,
            };

            var addTable = AddToDB(newTimetable);

            if (addTable != null) return new GeneralResponse(true, "Your timetable has been successfully created! Ready to explore your next adventure?");

            throw new NotImplementedException();
        }

        public async Task<GeneralResponse> ReturnCountryWeathers()
        {
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

        public async Task<List<Timetable>> GetTimetablesByTravelerEmailAsync(string travelerEmail)
        {
            var timetables = await dbContext.Timetables
                .Where(t => t.TravelerEmail == travelerEmail)
                .ToListAsync();

            return timetables;
        }

        public async Task<GeneralResponse> UpdateTimetableByIdAsync(string timetableId, UpdateTimetableDTO updatedTimetable)
        {
            var existingTimetable = await dbContext.Timetables.FindAsync(timetableId);

            if (existingTimetable == null)
            {
                return new GeneralResponse(false, "Timetable not found.");
            }

            // Update existing timetable with new values
            existingTimetable.TimelineTitle = updatedTimetable.TimelineTitle;
            existingTimetable.TimelineStartDate = updatedTimetable.TimelineStartDate;
            existingTimetable.TimelineEndDate = updatedTimetable.TimelineEndDate;
            existingTimetable.Country = updatedTimetable.Country;
            existingTimetable.Region = updatedTimetable.Region;

            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "Timetable updated! 🎉");
        }

        private async Task<T> AddToDB<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<GeneralResponse> DeleteTimetableByIDAsync(string timetableId)
        {
            if (timetableId == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var table = await FindTableById(timetableId);
            if (table == null) return new GeneralResponse(false, "The timeline with the given ID does not exist.");

            dbContext.Timetables.Remove(table);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "The timeline has been successfully deleted.");
        }

        private async Task<Timetable> FindTableById(string timetableID)
        {
            return await dbContext.Timetables.FirstOrDefaultAsync(_ => _.TimelineID!.Equals(timetableID!));
        }

        public async Task<GeneralResponse> GetTimetableAsync(string timetableId)
        {
            var timetableDetails = await FindTableById(timetableId);

            if (timetableDetails == null) return new GeneralResponse(false, "No timetable found.");

            var details = JsonConvert.SerializeObject(timetableDetails, Newtonsoft.Json.Formatting.Indented);

            return new GeneralResponse(true, details);
        }
    }
}
