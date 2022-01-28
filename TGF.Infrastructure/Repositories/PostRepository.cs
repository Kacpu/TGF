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
    public class PostRepository : IPostRepository
    {
        private AppDbContext _appDbContext;

        public PostRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Post> AddAsync(Post post)
        {
            try
            {
                _appDbContext.Posts.Add(post);
                _appDbContext.SaveChanges();
                return await Task.FromResult(post);
            }
            catch (Exception ex)
            {
                //await Task.FromException(ex);
                return null;
            }
        }

        public async Task<Post> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Posts.Include(p => p.Profile).FirstOrDefault(p => p.Id == id));
        }

        public async Task<IEnumerable<Post>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Posts.Include(p => p.Profile).Include(p => p.Character)
                .OrderByDescending(p => p.PublicationDate));
        }

        public async Task UpdateAsync(Post updatedP)
        {
            try
            {
                var pToUpdate = _appDbContext.Posts.FirstOrDefault(p => p.Id == updatedP.Id);

                pToUpdate.Title = updatedP.Title;
                pToUpdate.Content = updatedP.Content;
                pToUpdate.Annotation = updatedP.Annotation;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(Post post)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Posts.FirstOrDefault(p => p.Id == post.Id));
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
