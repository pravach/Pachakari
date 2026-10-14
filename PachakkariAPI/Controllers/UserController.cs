using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;

namespace Pachakkari.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public UserController(IUserService service,IValidator<RegisterUserDto> registerValidator,IValidator<LoginDto> loginValidator)
        {
            _userService = service;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
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

            var user = await _userService.LoginAsync(request);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }
            var user = new User
            {
                Id =id,
                Name=dto.Name,
                Email=dto.Email
            };
            var updatedUser = await _userService.UpdateUserAsync(user);
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

    }
}
