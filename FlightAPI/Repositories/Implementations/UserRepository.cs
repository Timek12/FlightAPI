﻿using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightAPI.Repositories.Implementations
{
    public class UserRepository(IApplicationDbContext db) : IUserRepository
    {
        private readonly IApplicationDbContext _db = db;

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}