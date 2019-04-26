using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Animerch.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("abc@xyz.com").Result == null)
            {
                User user = new User
                {
                    UserName = "Admin",
                    Email = "abc@xyz.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "Test@0").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
