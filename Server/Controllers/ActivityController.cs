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

    public class ActivityController(IActivity activityInterface) : ControllerBase
    {
        [HttpPost("create-event")]
        public async Task<IActionResult> AddActivityAsync(CreateActivityDTO activity)
        {
            if (activity == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await activityInterface.AddActivityAsync(activity);
            return Ok(result);
        }
        [HttpPut("update-event")]
        public async Task<IActionResult> UpdateActivityAsync(UpdateActivityDTO activity)
        {
            if (activity == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await activityInterface.UpdateActivityAsync(activity);
            return Ok(result);
        }

        [HttpDelete("delete-event/{activityId}")]
        public async Task<IActionResult> DeleteActivityAsync(string activityId)
        {
            if (string.IsNullOrEmpty(activityId))
            {
                return BadRequest("The activity ID must be provided.");
            }

            var result = await activityInterface.DeleteActivityAsync(activityId);
            return Ok(result);
        }

        [HttpGet("get-events/{timetableID}")]
        public async Task<IActionResult> GetActivityAsync(string timetableID)
        {
            if (string.IsNullOrEmpty(timetableID))
            {
                return BadRequest("The timetable ID must be provided.");
            }

            var result = await activityInterface.GetActivityAsync(timetableID);
            return Ok(result);
        }
    }
}
