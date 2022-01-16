using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface ICommentRepository
    {
        Task AddAsync(Comment comment);
        Task<Comment> GetAsync(int id);
        Task<IEnumerable<Comment>> BrowseAllAsync();
        Task UpdateAsync(Comment comment);
        Task DelAsync(Comment comment);
    }
}
