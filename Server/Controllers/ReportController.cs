using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;
using BaseLibrary.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ServerLibrary.Repositories.Implementations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using BaseLibrary.Responses;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(IReport ReportInterface) : ControllerBase
    {

        [HttpPost("create-report")]
        public async Task<IActionResult> AddReportAsync(ReportDTO report)
        {
            if (report == null)
            {
                return BadRequest("Report data is empty.");
            }

            var result = await ReportInterface.CreateReportAsync(report);
            return Ok(result);
        }

        [HttpPut("update-report")]
        public async Task<IActionResult> UpdateReportAsync(ReportDTO report)
        {
            if (report == null)
            {
                return BadRequest("Report data is empty.");
            }

            var result = await ReportInterface.UpdateReportAsync(report);
            return Ok(result);
        }

        [HttpDelete("delete-report/{reportId}")]
        public async Task<IActionResult> DeleteReportAsync(int reportId)
        {
            if (reportId <= 0)
            {
                return BadRequest("Invalid report ID.");
            }

            var result = await ReportInterface.DeleteReportAsync(reportId);
            return Ok(result);
        }

        [HttpGet("get-report")]
        public async Task<IActionResult> GetReportByEmailAsync(string reportEmail)
        {
            if (reportEmail == null || reportEmail == "")
            {
                return BadRequest("Invalid report email.");
            }

            var result = await ReportInterface.GetReportsByEmailAsync(reportEmail);
            return Ok(result);
        }

        [HttpGet("get-all-reports")]
        public async Task<IActionResult> GetAllReportsAsync()
        {
            var result = await ReportInterface.GetAllReportsAsync();
            return Ok(result);
        }

        [HttpGet("get-report/{reportId}")]
        public async Task<IActionResult> GetReportByIdAsync(int reportId)
        {
            if (reportId <= 0)
            {
                return BadRequest("Invalid report ID.");
            }

            var result = await ReportInterface.GetReportByIdAsync(reportId);
            if (result == null)
            {
                return NotFound("Report not found.");
            }

            return Ok(result);
        }

        [HttpPost("approve-report/{reportId}")]
        public async Task<IActionResult> ApproveReportAsync(int reportId)
        {
            if (reportId <= 0)
            {
                return BadRequest("Invalid report ID.");
            }

            var result = await ReportInterface.ApproveReportAsync(reportId);
            return Ok(result);
        }
    }
}
