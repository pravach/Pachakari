using Pachakkari.Application.DTOs;
using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Pachakkari.Application.DTOs;


namespace Pachakkari.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUserAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUsersByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<UserDto> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> BlockUserAsync(int id);
        Task<bool> UnblockUserAsync(int id);

        Task<UserDto?> RegisterAsync(RegisterUserDto request);
        Task<LoginResponseDto?> LoginAsync(LoginDto request);



    }
}
