using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface ICharacterStoryService
    {
        Task<CharacterStoryDTO> AddAsync(CharacterStoryDTO csDTO);
        //Task<CharacterStoryDTO> GetAsync(int cId, int sId);
        Task DelAsync(CharacterStoryDTO cs);
    }
}
