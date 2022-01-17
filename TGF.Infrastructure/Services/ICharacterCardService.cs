using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface ICharacterCardService
    {
        Task<CharacterCardDTO> AddAsync(CharacterCardDTO characterCardDTO);
        Task<CharacterCardDTO> GetAsync(int id);
        Task<IEnumerable<CharacterCardDTO>> BrowseAllAsync();
        Task UpdateAsync(CharacterCardDTO characterCardDTO);
        Task DelAsync(CharacterCardDTO characterCardDTO);
    }
}
