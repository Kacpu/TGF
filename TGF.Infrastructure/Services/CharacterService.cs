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
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<CharacterDTO> AddAsync(CharacterDTO character)
        {
            var c = await _characterRepository.AddAsync(await ToDomain(character));
            return c != null ? ToDTO(c) : null;
        }

        public async Task<CharacterDTO> GetAsync(int id)
        {
            var character = await _characterRepository.GetAsync(id);

            if (character == null)
            {
                return null;
            }

            return ToDTO(character);
        }

        public async Task<IEnumerable<CharacterDTO>> BrowseAllAsync()
        {
            var character = await _characterRepository.BrowseAllAsync();

            if (character == null)
            {
                return null;
            }

            return character.Select(c => ToDTO(c));
        }

        public async Task UpdateAsync(CharacterDTO characterDTO)
        {
            if (characterDTO != null)
            {
                await _characterRepository.UpdateAsync(await ToDomain(characterDTO));
            }
        }

        public async Task DelAsync(CharacterDTO characterDTO)
        {
            if (characterDTO != null)
            {
                await _characterRepository.DelAsync(await ToDomain(characterDTO));
            }
        }

        private CharacterDTO ToDTO(Character c)
        {
            return new CharacterDTO()
            {
                Id = c.Id,
                Name = c.Name,
                ProfileId = c.ProfileId,
                CharacterCard = c.CharacterCard != null ? new CharacterCardDTO()
                {
                    Id = c.CharacterCard.Id,
                    AppearanceDescription = c.CharacterCard.AppearanceDescription,
                    CharacterDescription = c.CharacterCard.CharacterDescription,
                    History = c.CharacterCard.History
                } : null
            };
        }

        private async Task<Character> ToDomain(CharacterDTO cDTO)
        {
            return new Character()
            {
                Id = cDTO.Id,
                Name = cDTO.Name,
                ProfileId = cDTO.ProfileId
            };
        }
    }
}
