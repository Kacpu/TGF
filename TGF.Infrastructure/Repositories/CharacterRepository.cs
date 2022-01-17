using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;

namespace TGF.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private AppDbContext _appDbContext;

        public CharacterRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Character> AddAsync(Character character)
        {
            try
            {
                _appDbContext.Characters.Add(character);
                _appDbContext.SaveChanges();
                return await Task.FromResult(character);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
                return null;
            }
        }

        public async Task<Character> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Characters.FirstOrDefault(c => c.Id == id));
        }

        public async Task<IEnumerable<Character>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Characters);
        }

        public async Task UpdateAsync(Character updatedC)
        {
            try
            {
                var cToUpdate = _appDbContext.Characters.FirstOrDefault(c => c.Id == updatedC.Id);

                cToUpdate.Name = updatedC.Name;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(Character character)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Characters.FirstOrDefault(c => c.Id == character.Id));
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
