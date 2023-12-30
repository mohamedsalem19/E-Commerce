using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository.Identity
{
    public class ApplicationIdentityDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext>options) :base(options)
        {

        }
       
    }
}
