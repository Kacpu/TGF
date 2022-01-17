using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;

namespace TGF.Infrastructure.Repositories
{
    class CharacterCardRepository : ICharacterCardRepository
    {
        private AppDbContext _appDbContext;

        public CharacterCardRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CharacterCard> AddAsync(CharacterCard characterCard)
        {
            try
            {
                _appDbContext.CharacterCards.Add(characterCard);
                _appDbContext.SaveChanges();
                return await Task.FromResult(characterCard);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
                return null;
            }
        }

        public async Task<CharacterCard> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.CharacterCards.FirstOrDefault(c => c.Id == id));
        }

        public async Task<IEnumerable<CharacterCard>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.CharacterCards);
        }

        public async Task UpdateAsync(CharacterCard updatedC)
        {
            try
            {
                var cToUpdate = _appDbContext.CharacterCards.FirstOrDefault(c => c.Id == updatedC.Id);

                cToUpdate.History = updatedC.History;
                cToUpdate.AppearanceDescription = updatedC.AppearanceDescription;
                cToUpdate.CharacterDescription = updatedC.CharacterDescription;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(CharacterCard characterCard)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.CharacterCards.FirstOrDefault(c => c.Id == characterCard.Id));
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
