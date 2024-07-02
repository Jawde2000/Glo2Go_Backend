using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.Models;
using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IReport
    {
        public Task<List<ReportDTO>> GetAllReportsAsync();
        public Task<ReportDTO> GetReportByIdAsync(int reportId);

        public Task<GeneralResponse> CreateReportAsync(ReportDTO report);
        public Task<GeneralResponse> DeleteReportAsync(int reportId);
        public Task<GeneralResponse> UpdateReportAsync(ReportDTO report);

        public Task<GeneralResponse> ApproveReportAsync(int id);
        public Task<List<ReportDTO>> GetReportsByEmailAsync(string reportEmail);

    }
}
