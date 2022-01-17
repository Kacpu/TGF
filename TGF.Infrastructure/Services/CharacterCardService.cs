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
    public class CharacterCardService : ICharacterCardService
    {
        private readonly ICharacterCardRepository _characterCardRepository;

        public CharacterCardService(ICharacterCardRepository characterCardRepository)
        {
            _characterCardRepository = characterCardRepository;
        }

        public async Task<CharacterCardDTO> AddAsync(CharacterCardDTO characterCard)
        {
            var c = await _characterCardRepository.AddAsync(await ToDomain(characterCard));
            return c != null ? ToDTO(c) : null;
        }

        public async Task<CharacterCardDTO> GetAsync(int id)
        {
            var characterCard = await _characterCardRepository.GetAsync(id);

            if (characterCard == null)
            {
                return null;
            }

            return ToDTO(characterCard);
        }

        public async Task<IEnumerable<CharacterCardDTO>> BrowseAllAsync()
        {
            var characterCards = await _characterCardRepository.BrowseAllAsync();

            if (characterCards == null)
            {
                return null;
            }

            return characterCards.Select(c => ToDTO(c));
        }

        public async Task UpdateAsync(CharacterCardDTO characterCardDTO)
        {
            if (characterCardDTO != null)
            {
                await _characterCardRepository.UpdateAsync(await ToDomain(characterCardDTO));
            }
        }

        public async Task DelAsync(CharacterCardDTO characterCardDTO)
        {
            if (characterCardDTO != null)
            {
                await _characterCardRepository.DelAsync(await ToDomain(characterCardDTO));
            }
        }

        private CharacterCardDTO ToDTO(CharacterCard c)
        {
            return new CharacterCardDTO()
            {
                Id = c.Id,
                History = c.History,
                AppearanceDescription = c.AppearanceDescription,
                CharacterDescription = c.CharacterDescription
            };
        }

        private async Task<CharacterCard> ToDomain(CharacterCardDTO cDTO)
        {
            return new CharacterCard()
            {
                Id = cDTO.Id,
                History = cDTO.History,
                AppearanceDescription = cDTO.AppearanceDescription,
                CharacterDescription = cDTO.CharacterDescription
            };
        }
    }
}
