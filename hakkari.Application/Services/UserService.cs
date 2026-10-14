using Pachakkari.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Pachakkari.Domain.Entities;
using BCrypt.Net;
using Pachakkari.Application.DTOs;


namespace Pachakkari.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<List<UserDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Type = user.Type,
                IsBlocked = user.IsBlocked
            }).ToList();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUsersByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task<UserDto> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            await _userRepository.UpdateAsync(existingUser);
            return new UserDto
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Email = existingUser.Email,
                Type = existingUser.Type,
                IsBlocked = existingUser.IsBlocked
            };
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserDto?> RegisterAsync(RegisterUserDto request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return null;
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Type = "user",
                IsBlocked = false,


            };
            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Type = user.Type,
                IsBlocked = user.IsBlocked
            };

        }

        public async Task<LoginResponseDto?>LoginAsync(LoginDto request)
        {
           var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) {

                return null;

            }
            bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);

            if (!passwordValid)
            {
                return null;

            }

            if(user.IsBlocked)
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Type = user.Type,
                    IsBlocked = user.IsBlocked
                },
                Token = token
            };

        }


        public async Task<bool> BlockUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsBlocked = true;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> UnblockUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsBlocked = false;

            await _userRepository.UpdateAsync(user);

            return true;
        }

    }
}
