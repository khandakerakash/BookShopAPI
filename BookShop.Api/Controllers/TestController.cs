using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public TestController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }
        // seed the database with some dummy role and user
        // GET: api/test
        [HttpGet]
        public async Task Get()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "owner"
            });
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "manager"
            });
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "customer"
            });

            var owner = await userManager.CreateAsync(new ApplicationUser()
            {
                Email = "akash.cse10@gmail.com",
                PhoneNumber = "01670047320",
                UserName = "akash.cse10@gmail.com"
            }, "Akash&core007");

            if (owner.Succeeded)
            {

                var nowInsertedUser = await userManager.FindByEmailAsync("akash.cse10@gmail.com");
                var roleInsert = await userManager.AddToRoleAsync(nowInsertedUser, "owner");
            }

            var manager = await userManager.CreateAsync(new ApplicationUser()
            {
                Email = "akkubaby.cse10@gmail.com",
                PhoneNumber = "01911946813",
                UserName = "akkubaby.cse10@gmail.com"
            }, "Akash&core007");

            if (manager.Succeeded)
            {

                var nowInsertedUser1 = await userManager.FindByEmailAsync("akkubaby.cse10@gmail.com");
                var roleInsert1 = await userManager.AddToRoleAsync(nowInsertedUser1, "manager");
            }

            //var customer = await userManager.CreateAsync(new ApplicationUser()
            //{
            //    Email = "khandakerakash007@gmail.com",
            //    PhoneNumber = "01911946812",
            //    UserName = "khandakerakash007@gmail.com"
            //}, "Akash&core007");

            //if (customer.Succeeded)
            //{

            //    var nowInsertedUser1 = await userManager.FindByEmailAsync("khandakerakash007@gmail.com");
            //    var roleInsert1 = await userManager.AddToRoleAsync(nowInsertedUser1, "customer");
            //}
        }
    }
}