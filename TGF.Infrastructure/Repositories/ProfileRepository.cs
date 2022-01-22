using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private AppDbContext _appDbContext;

        public ProfileRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Profile> AddAsync(Profile profile)
        {
            try
            {
                _appDbContext.Profiles.Add(profile);
                _appDbContext.SaveChanges();
                return await Task.FromResult(profile);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
                return null;
            }
        }

        public async Task<Profile> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Profiles.Include(p => p.Characters).FirstOrDefault(p => p.Id == id));
        }

        public async Task<IEnumerable<Profile>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Profiles);
        }

        public async Task<Profile> FindByUsername(string username)
        {
            return await Task.FromResult(_appDbContext.Profiles.Include(p => p.AppUser).Include(p => p.Characters)
                .FirstOrDefault(p => p.AppUser.UserName == username));
        }

        public async Task UpdateAsync(Profile updatedP)
        {
            try
            {
                var pToUpdate = _appDbContext.Profiles.FirstOrDefault(p => p.Id == updatedP.Id);

                pToUpdate.Name = updatedP.Name;
                pToUpdate.Description = updatedP.Description;
                pToUpdate.LastSeen = updatedP.LastSeen;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(Profile profile)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Profiles.FirstOrDefault(p => p.Id == profile.Id));
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    }
}
