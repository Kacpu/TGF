using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;

namespace TGF.Infrastructure.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private AppDbContext _appDbContext;

        public StoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Story> AddAsync(Story story)
        {
            try
            {
                _appDbContext.Stories.Add(story);
                _appDbContext.SaveChanges();
                return await Task.FromResult(story);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
                return null;
            }
        }

        public async Task<Story> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Stories.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Story>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Stories);
        }

        public async Task UpdateAsync(Story updatedS)
        {
            try
            {
                var sToUpdate = _appDbContext.Stories.FirstOrDefault(s => s.Id == updatedS.Id);

                sToUpdate.Title = updatedS.Title;
                sToUpdate.CreationDate = updatedS.CreationDate;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(Story story)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Stories.FirstOrDefault(s => s.Id == story.Id));
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
