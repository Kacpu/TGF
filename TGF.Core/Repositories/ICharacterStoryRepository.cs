using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface ICharacterStoryRepository
    {
        Task<CharacterStory> AddAsync(CharacterStory cs);
        //Task<CharacterStory> GetAsync(int cId, int sId);
        Task DelAsync(CharacterStory cs);
    }
}
