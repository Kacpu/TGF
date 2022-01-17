using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface ICharacterService
    {
        Task<CharacterDTO> AddAsync(CharacterDTO characterDTO);
        Task<CharacterDTO> GetAsync(int id);
        Task<IEnumerable<CharacterDTO>> BrowseAllAsync();
        Task UpdateAsync(CharacterDTO characterDTO);
        Task DelAsync(CharacterDTO characterDTO);
    }
}
