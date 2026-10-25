using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using System.Security.Claims;

namespace Pachakkari.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public UserController(IUserService service, IValidator<RegisterUserDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userService = service;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        private int? GetUserId()
        {
            var userIdValue = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdValue, out var userId))
            {
                return userId;
            }

            return null;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto request)
        {
            var validationResult =
                await _registerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => new
                {
                    Field = error.PropertyName,
                    Message = error.ErrorMessage
                }));
            }

            var user = await _userService.RegisterAsync(request);

            if (user == null)
            {
                return BadRequest("Email already exists");
            }

            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var validationResult =
                await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => new
                {
                    Field = error.PropertyName,
                    Message = error.ErrorMessage
                }));
            }

            var loginResponse = await _userService.LoginAsync(request);

            if (loginResponse == null)
            {
                return Unauthorized("Invalid email or password, or account is blocked.");
            }

            // Store the JWT in an HttpOnly cookie
            Response.Cookies.Append(
                "access_token",
                loginResponse.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                    Path = "/"
                });

            // Return user details, but do not expose the token in the response body
            return Ok(loginResponse.User);
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            var user = await _userService.GetUserByIdAsync(
                userId.Value);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
    UpdateUserDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            var existingUser = await _userService.GetUserByIdAsync(
                userId.Value);

            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            existingUser.Name = dto.Name;
            existingUser.Email = dto.Email;

            var updatedUser = await _userService.UpdateUserAsync(
                existingUser);

            return Ok(updatedUser);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.DeleteUserAsync(id);

            return Ok("User deleted successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/block")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var result = await _userService.BlockUserAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            return Ok(new
            {
                message = "User blocked successfully"
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/unblock")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            var result = await _userService.UnblockUserAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            return Ok(new
            {
                message = "User unblocked successfully"
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(
                "access_token",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/"
                });

            return Ok(new { message = "Logged out successfully." });

        }
    }
}
