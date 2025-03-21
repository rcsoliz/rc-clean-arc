﻿using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context; 

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Access(User user)
        {
            var userByEmail = await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
            if (userByEmail == null || !userByEmail.VerifyPassword(user.Password))
                return new User();

       
            return userByEmail;
        }

        public async Task<User> AddAsync(UserModel entity)
        {
            var user = new User();
            user.Username = entity.Username;
            user.Email = entity.Email;
            user.SetPassword(entity.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

    }
}
