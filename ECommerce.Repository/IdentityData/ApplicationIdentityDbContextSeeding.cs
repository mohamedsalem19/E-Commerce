using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository.IdentityData
{
    public static class ApplicationIdentityDbContextSeeding
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Mohamed_Mahmoud",
                    Email = "eng.mohamedmahmoud24@gmail.com",
                    UserName = "eng.mohamedmahmoud24",
                    PhoneNumber = "1234567890"
                };
                await userManager.CreateAsync(user,"P@ssw0rd");
            }
        }
    }
}
