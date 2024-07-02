using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Implementations
{
    public class ReportRepository : IReport
    {
        private readonly Glo2GoDbContext dbContext;

        public ReportRepository(Glo2GoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GeneralResponse> CreateReportAsync(ReportDTO report)
        {
            if (report == null)
            {
                return new GeneralResponse(false, "Report data is empty.");
            }

            var newReport = new Report()
            {
                SiteID = report.SiteID,
                ReportTitle = report.ReportTitle,
                ReportFeedback = report.ReportFeedback,
                ReportType = report.ReportType,
                ReportEmail = report.ReportEmail,
                IsApproved = false,
                IsReviewedByAdmin = false,
            };

            var addedReport = await AddToDB(newReport);

            if (addedReport != null)
            {
                return new GeneralResponse(true, "Report successfully added.");
            }
            else
            {
                throw new Exception("Failed to add report."); // You can handle this more gracefully
            }
        }

        public async Task<GeneralResponse> DeleteReportAsync(int reportId)
        {
            var report = await dbContext.Reports.FindAsync(reportId);

            if (report == null)
            {
                return new GeneralResponse(false, "Report not found.");
            }

            dbContext.Reports.Remove(report);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "Report deleted successfully.");
        }

        public async Task<List<ReportDTO>> GetAllReportsAsync()
        {
            var reports = await dbContext.Reports.ToListAsync();

            if (reports == null || reports.Count == 0)
            {
                return new List<ReportDTO>();
            }

            var reportDTOs = reports.Select(report => new ReportDTO
            {
                ReportId = report.ReportId,
                ReportTitle = report.ReportTitle,
                ReportFeedback = report.ReportFeedback,
                ReportType = report.ReportType,
                SiteID = report.SiteID,
                IsApproved = report.IsApproved,
                IsReviewedByAdmin = report.IsReviewedByAdmin,
            }).ToList();

            return reportDTOs;
        }

        public async Task<ReportDTO> GetReportByIdAsync(int reportId)
        {
            var report = await dbContext.Reports.FindAsync(reportId);

            if (report == null)
            {
                return null; // Or you can return a specific response indicating report not found
            }

            var reportDTO = new ReportDTO
            {
                ReportId = report.ReportId,
                ReportTitle = report.ReportTitle,
                ReportFeedback = report.ReportFeedback,
                ReportType = report.ReportType,
                SiteID = report.SiteID,
                IsApproved = report.IsApproved,
                IsReviewedByAdmin = report.IsReviewedByAdmin,
            };

            return reportDTO;
        }

        public async Task<GeneralResponse> UpdateReportAsync(ReportDTO report)
        {
            var existingReport = await dbContext.Reports.FindAsync(report.ReportId);

            if (existingReport == null)
            {
                return new GeneralResponse(false, "Report not found.");
            }

            existingReport.ReportTitle = report.ReportTitle ?? existingReport.ReportTitle;
            existingReport.ReportFeedback = report.ReportFeedback ?? existingReport.ReportFeedback;
            existingReport.ReportType = report.ReportType ?? existingReport.ReportType;

            dbContext.Reports.Update(existingReport);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "Report updated successfully.");
        }

        private async Task<T> AddToDB<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<List<ReportDTO>> GetReportsByEmailAsync(string reportEmail)
        {
            var reports = await dbContext.Reports
                .Where(r => r.ReportEmail == reportEmail)
                .ToListAsync();

            if (reports == null || reports.Count == 0)
            {
                return new List<ReportDTO>(); // Or handle as per your application's logic
            }

            var reportDTOs = reports.Select(report => new ReportDTO
            {
                ReportId = report.ReportId,
                ReportTitle = report.ReportTitle,
                ReportFeedback = report.ReportFeedback,
                ReportType = report.ReportType,
                SiteID = report.SiteID,
                IsApproved = report.IsApproved,
                IsReviewedByAdmin = report.IsReviewedByAdmin,
            }).ToList();

            return reportDTOs;
        }

        public async Task<GeneralResponse> ApproveReportAsync(int reportId)
        {
            var report = await dbContext.Reports.FindAsync(reportId);

            if (report == null)
            {
                return new GeneralResponse(false, "Report not found.");
            }

            report.IsApproved = true;
            report.IsReviewedByAdmin = true;

            dbContext.Reports.Update(report);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "Report approved successfully.");
        }

    }
}
