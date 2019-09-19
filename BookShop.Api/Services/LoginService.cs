using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookShop.Api.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginService(ApplicationDbContext context, IConfiguration configuration
            , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetUserByPhoneNumber(string phoneNumber)
        {
            var info = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            return info;
        }

        public async Task<bool> EmailExists(string email, CancellationToken token)
        {
            if (String.IsNullOrEmpty(email))
            {
                return true;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> PhoneNumberExists(string phoneNumber, CancellationToken token)
        {
            if (String.IsNullOrEmpty(phoneNumber))
            {
                return true;
            }

            var user = await GetUserByPhoneNumber(phoneNumber);
            if (user == null)
            {
                return true;
            }

            return false;
        }
    }
}
