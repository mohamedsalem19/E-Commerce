using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce.API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user  = await userManager.Users.Include(U=>U.address).FirstOrDefaultAsync(u => u.Email == email);

            return user;

        }












    }
}
