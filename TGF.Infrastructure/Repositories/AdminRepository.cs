using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;

namespace TGF.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private AppDbContext _appDbContext;

        public AdminRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            List<AppUser> admins = new List<AppUser>();
            var adminRoles = _appDbContext.UserRoles.Where(ur => ur.RoleId.Equals("1")).ToList();

            foreach(var adminRole in adminRoles)
            {
                var user = _appDbContext.Users.FirstOrDefault(u => u.Id == adminRole.UserId);
                admins.Add(user);

            }
            
            return await Task.FromResult(admins);
        }
    }
}
