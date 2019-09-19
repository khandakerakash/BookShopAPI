using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Api.Models;

namespace BookShop.Api.Services
{
    public interface ILoginService
    {
        Task<ApplicationUser> GetUserByPhoneNumber(string phoneNumber);
        Task<bool> EmailExists(string email, CancellationToken token);
        Task<bool> PhoneNumberExists(string phoneNumber, CancellationToken token);
    }
}
