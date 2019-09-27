using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public TestController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }
        // seed the database with some dummy role and user
        // GET: api/test
        [HttpGet]
        public async Task Get()
        {
            //await  InsertUser();

            //  await CategoryListGenerate();
            // return null;
        }

        private async Task CategoryListGenerate()
        {
            var userlist = await _context.Users.ToListAsync();


            var categoryFaker = new Faker<Category>()
                .RuleFor(o => o.Name, f => f.Name.FullName())
                .RuleFor(o => o.Description, f => f.Lorem.Paragraph());
            var categoryList = categoryFaker.Generate(1000).ToList();
            foreach (var category in categoryList)
            {
                var random = new Random();
                int index = random.Next(userlist.Count);
                category.ApplicationUserId = userlist.ElementAt(index).Id;
                await _context.Categories.AddAsync(category);
            }

            await _context.SaveChangesAsync();
        }

        private async Task InsertUser()
        {
            await _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = "owner"
            });
            await _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = "manager"
            });
            await _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = "customer"
            });

            var list = new List<string> { "owner", "manager", "customer" };
            var userList = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    PhoneNumber = "01670047320",
                    UserName = "akash.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047321",
                    UserName = "akash1.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047322",
                    UserName = "akash2.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047323",
                    UserName = "akash3.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047324",
                    UserName = "akash4.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047325",
                    UserName = "akash5.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047326",
                    UserName = "akash6.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047327",
                    UserName = "akash7.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047328",
                    UserName = "akash8.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047329",
                    UserName = "akash9.cse10@gmail.com"
                },
                new ApplicationUser()
                {
                    PhoneNumber = "01670047330",
                    UserName = "akash10.cse10@gmail.com"
                },
            };

            foreach (var aUser in userList)
            {
                var owner = await userManager.CreateAsync(new ApplicationUser()
                {
                    Email = aUser.UserName,
                    PhoneNumber = aUser.PhoneNumber,
                    UserName = aUser.UserName,
                }, "Akash&core007");

                if (owner.Succeeded)
                {
                    var random = new Random();
                    int index = random.Next(list.Count);
                    var nowInsertedUser = await userManager.FindByEmailAsync(aUser.UserName);
                    var roleInsert = await userManager.AddToRoleAsync(nowInsertedUser, list[index]);
                }
            }
        }
    }
}