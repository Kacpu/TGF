using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;
using TGF.Core.Repositories;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public class CharacterStoryService : ICharacterStoryService
    {
        private readonly ICharacterStoryRepository _characterStoryRepository;

        public CharacterStoryService(ICharacterStoryRepository characterStoryRepository)
        {
            _characterStoryRepository = characterStoryRepository;
        }

        public async Task<CharacterStoryDTO> AddAsync(CharacterStoryDTO csDTO)
        {
            var cs = await _characterStoryRepository.AddAsync(ToDomain(csDTO));
            return cs != null ? ToDTO(cs) : null;
        }

        //public async Task<CharacterStoryDTO> GetAsync(int cId, int sId)
        //{
        //    var cs = await _characterStoryRepository.GetAsync(cId, sId);

        //    if (cs == null)
        //    {
        //        return null;
        //    }

        //    return ToDTO(cs);
        //}

        public async Task DelAsync(CharacterStoryDTO csDTO)
        {
            if (csDTO != null)
            {
                await _characterStoryRepository.DelAsync(ToDomain(csDTO));
            }
        }

        private CharacterStoryDTO ToDTO(CharacterStory cs)
        {
            return new CharacterStoryDTO()
            {
                CharactersId = cs.CharacterId,
                StoriesId = cs.StorieId
            };
        }

        private CharacterStory ToDomain(CharacterStoryDTO csDTO)
        {
            return new CharacterStory()
            {
                CharacterId = csDTO.CharactersId,
                StorieId = csDTO.StoriesId
            };
        }
    }
}
