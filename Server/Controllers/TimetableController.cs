using BaseLibrary.DTOs;
using BaseLibrary.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ServerLibrary.Repositories.Implementations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using BaseLibrary.Responses;
using ServerLibrary.Repositories.Contracts;
using System.Xml;
using ServerLibrary.Migrations;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController(ITimetable timetableInterface) : ControllerBase
    {
        [HttpPost("create-timetable")]
        public async Task<IActionResult> CreateTimetableAsync(CreateTimetableDTO timetable)
        {
            if (timetable == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await timetableInterface.CreateTimetableAsync(timetable);
            return Ok(result);
        }

        [HttpGet("get-timetable")]
        public async Task<IActionResult> GetTimetablesByTravelerEmailAsync(string travelerEmail)
        {
            if (travelerEmail == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await timetableInterface.GetTimetablesByTravelerEmailAsync(travelerEmail);
            return Ok(result);
        }

        [HttpPut("update-timetable")]
        public async Task<IActionResult> UpdateTimetableByIdAsync(string timetableId, UpdateTimetableDTO updatedTimetable)
        {
            if (timetableId == null || updatedTimetable == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await timetableInterface.UpdateTimetableByIdAsync(timetableId, updatedTimetable);
            return Ok(result);
        }

        [HttpDelete("delete-timetable")]
        public async Task<IActionResult> DeleteTimetableByIDAsync(string timetableId)
        {
            if (timetableId == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await timetableInterface.DeleteTimetableByIDAsync(timetableId);
            return Ok(result);
        }

        [HttpGet("get-single-timetable")]
        public async Task<IActionResult> GetTimetableAsync(string timetableId)
        {
            if (timetableId == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await timetableInterface.GetTimetableAsync(timetableId);
            return Ok(result);
        }
    }
}
