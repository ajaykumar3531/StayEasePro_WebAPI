using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using AuthGuardPro_Application.Repos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/User")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user. Requires authorization.
    /// </summary>
    /// <param name="request">The user creation request containing username, email, and password.</param>
    /// <returns>A response containing the created user's details.</returns>
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        try
        {
            var response = await _userService.CreateUser(request);
            if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                return Ok(response);
            else
                return BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }



    /// <summary>
    /// Logs in a user. No authorization required.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <returns>A response containing the user's login status.</returns>
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        try
        {
            var response = await _userService.LoginUser(request);
            if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                return Ok(response);
            else
                return BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        try
        {
            var response = await _userService.ForgotPassword(request);
            if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                return Ok(response);
            else
                return BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [Authorize]
    [HttpPost("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount(DeleteRequest request)
    {
        try
        {
            var response = await _userService.DeleteUser(request);
            if (response.StatusCode == StatusCodes.Status200OK && !string.IsNullOrEmpty(response.StatusMessage))
                return Ok(response);
            else
                return BadRequest(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


}
