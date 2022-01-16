using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface ICharacterRepository
    {
        Task AddAsync(Character character);
        Task<Character> GetAsync(int id);
        Task<IEnumerable<Character>> BrowseAllAsync();
        Task UpdateAsync(Character character);
        Task DelAsync(Character character);
    }
}
