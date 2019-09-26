using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Api.RequestResponse.Request
{
    public class LoginUserInformation
    {
        public ClaimsPrincipal Myuser { get; set; }
    }
}
