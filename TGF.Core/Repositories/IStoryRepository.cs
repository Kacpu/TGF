using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface IStoryRepository
    {
        Task<Story> AddAsync(Story story);
        Task<Story> GetAsync(int id);
        Task<IEnumerable<Story>> BrowseAllAsync();
        Task UpdateAsync(Story story);
        Task DelAsync(Story story);
    }
}
