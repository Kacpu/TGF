using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;

namespace TGF.Infrastructure.Repositories
{
    public class CharacterStoryRepository : ICharacterStoryRepository
    {
        private AppDbContext _appDbContext;

        public CharacterStoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CharacterStory> AddAsync(CharacterStory cs)
        {
            try
            {
                Story story = _appDbContext.Stories.Include(s => s.Characters).FirstOrDefault(s => s.Id == cs.StorieId);
                Character character = _appDbContext.Characters.Find(cs.CharacterId);
                if(story != null && character != null)
                {
                    story.Characters.Add(character);
                    _appDbContext.SaveChanges();
                    return await Task.FromResult(cs);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public async Task<CharacterStory> GetAsync(int cId, int sId)
        //{
        //    return await Task.FromResult(_appDbContext.CharacterStory.FirstOrDefault(x => x.CharactersId == cId && x.StoriesId == sId));
        //}

        public async Task DelAsync(CharacterStory cs)
        {
            try
            {
                Story story = _appDbContext.Stories.Include(s => s.Characters).FirstOrDefault(s => s.Id == cs.StorieId);
                Character character = _appDbContext.Characters.Find(cs.CharacterId);
                if (story != null && character != null)
                {
                    story.Characters.Remove(character);
                    _appDbContext.SaveChanges();
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }
    }
}
