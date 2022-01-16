using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        Task<Post> GetAsync(int id);
        Task<IEnumerable<Post>> BrowseAllAsync();
        Task UpdateAsync(Post post);
        Task DelAsync(Post post);
    }
}
