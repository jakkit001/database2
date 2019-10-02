using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace database2.Data
{
    public class simpledata
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            var userManage = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            string[] roles = new string[] { "Administrator","User"};
            
            foreach (var role in roles)
            {
                var isExist = await roleManager.RoleExistsAsync(role);
                if(!isExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var adminUser = new IdentityUser
            {
                Email = "jakkit-st@rmutsb.ac.th",
                UserName = "jakkit-st@rmutsb.ac.th",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var currentUser = await userManage.FindByEmailAsync(adminUser.Email);
            if (currentUser == null)
            {
                await userManage.CreateAsync(adminUser, "St-360408241001");
                currentUser = await userManage.FindByEmailAsync(adminUser.Email);
            }
            var isAdmin = await userManage.IsInRoleAsync(currentUser, "Administrator");
            if (!isAdmin)
            {
                await userManage.AddToRolesAsync(currentUser, roles);
            }
            var containSampleBook = await dbContext.books.AnyAsync(b => b.name == "sample book");
            if (!containSampleBook)
            {
                dbContext.books.Add(new Models.book
                {
                    name = "sample book",
                    price = 100m
                });
            }
            await dbContext.SaveChangesAsync();


        }   
    }
}
