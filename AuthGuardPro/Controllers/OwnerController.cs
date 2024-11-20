using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.Repos.Contracts;
using StayEasePro_Domain.DTO_s.DTO;
using StayEasePro_Domain.DTO_s.Requests;

namespace StayEasePro.Controllers
{
    //[Authorize]
    [Route("api/Owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IOwnerService _ownerService;

        public OwnerController(ILoggerService loggerService, IOwnerService ownerService)
        {
            _loggerService = loggerService;
            _ownerService = ownerService;
        }

        [HttpPost("CreateProperty")]
        public async Task<IActionResult> CreateProperty(CreatePropertyRequest request)
        {
            try
            {
                // string UserID = GlobalUserContext.UserID.ToUpper();
                string UserID = "2FFD76E6-4638-4214-8963-5DC5B6006C6A";
                var response = await _ownerService.CreateProperty(request, UserID);
                if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _loggerService.LocalLogs(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("UpdateProperty")]
        public async Task<IActionResult> UpdateProperty(UpdatePropertyRequest request)
        {
            try
            {
                // string UserID = GlobalUserContext.UserID.ToUpper();
                string UserID = "2FFD76E6-4638-4214-8963-5DC5B6006C6A";
                var response = await _ownerService.UpdateProperty(request, UserID);
                if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                await _loggerService.LocalLogs(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
