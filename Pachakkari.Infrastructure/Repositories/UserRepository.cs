using System;
using System.Collections.Generic;
using System.Text;
using Pachakkari.Application.Interfaces;
using Pachakkari.Infrastructure.Data;
using Pachakkari.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace Pachakkari.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly PachakkariDbContext _context;

        public UserRepository(PachakkariDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>>GetAllAsync()
        {
            return await _context.Users.ToListAsync();

        }

        public async Task<User?>GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?>GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }




    }
}
