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
    }
}
