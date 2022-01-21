using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface IProfileRepository
    {
        Task<Profile> AddAsync(Profile profile);
        Task<Profile> GetAsync(int id);
        Task<IEnumerable<Profile>> BrowseAllAsync();
        Task<Profile> FindByUsername(string username);
        Task UpdateAsync(Profile profile);
        Task DelAsync(Profile profile);
    }
}
