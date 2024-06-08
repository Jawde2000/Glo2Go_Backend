using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Contracts
{
    public interface ITimetable
    {
        public Task<GeneralResponse> CreateTimetableAsync(CreateTimetableDTO timetable);
        public Task<GeneralResponse> ReturnCountryWeathers();
        public Task<GeneralResponse> UpdateTimetableByIdAsync(string timetableId, UpdateTimetableDTO updatedTimetable);
        public Task<List<Timetable>> GetTimetablesByTravelerEmailAsync(string travelerEmail);

        public Task<GeneralResponse> DeleteTimetableByIDAsync(string timetableId);

        public Task<GeneralResponse> GetTimetableAsync(string timetableId);
    }
}
