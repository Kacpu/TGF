using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;

namespace TGF.Infrastructure.Repositories
{
    class CommentRepository : ICommentRepository
    {
        private AppDbContext _appDbContext;

        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            try
            {
                _appDbContext.Comments.Add(comment);
                _appDbContext.SaveChanges();
                return await Task.FromResult(comment);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
                return null;
            }
        }

        public async Task<Comment> GetAsync(int id)
        {
            return await Task.FromResult(_appDbContext.Comments.FirstOrDefault(c => c.Id == id));
        }

        public async Task<IEnumerable<Comment>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Comments);
        }

        public async Task UpdateAsync(Comment updatedC)
        {
            try
            {
                var cToUpdate = _appDbContext.Comments.FirstOrDefault(c => c.Id == updatedC.Id);

                cToUpdate.PublicationDate = updatedC.PublicationDate;
                cToUpdate.Content = updatedC.Content;

                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task DelAsync(Comment comment)
        {
            try
            {
                _appDbContext.Remove(_appDbContext.Comments.FirstOrDefault(c => c.Id == comment.Id));
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
