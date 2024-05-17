using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface ITimetable
    {
        public Task<GeneralResponse> CreateTimetableAsync(CreateTimetableDTO timetable);
    }
}
