using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface ICharacterCardRepository
    {
        Task<CharacterCard> AddAsync(CharacterCard characterCard);
        Task<CharacterCard> GetAsync(int id);
        Task<IEnumerable<CharacterCard>> BrowseAllAsync();
        Task UpdateAsync(CharacterCard characterCard);
        Task DelAsync(CharacterCard characterCard);
    }
}
