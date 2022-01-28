using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGF.Infrastructure.Commands;
using TGF.Infrastructure.DTO;
using TGF.Infrastructure.Services;

namespace TGF.WebAPI.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    public class CharacterCardController : Controller
    {
        private readonly ICharacterCardService _characterCardService;

        public CharacterCardController(ICharacterCardService characterCardService)
        {
            _characterCardService = characterCardService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacterCard([FromBody] CreateCharacterCard characterCard)
        {
            if (characterCard == null)
            {
                return BadRequest();
            }

            CharacterCardDTO characterCardDTO = new CharacterCardDTO()
            {
                History = characterCard.History,
                AppearanceDescription = characterCard.AppearanceDescription,
                CharacterDescription = characterCard.CharacterDescription,
                CharacterId = characterCard.CharacterId
            };

            var c = await _characterCardService.AddAsync(characterCardDTO);

            if (c == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetCharacterCard), new { id = c.Id }, c);
        }

        //https://localhost:5001/comment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterCard(int id)
        {
            CharacterCardDTO characterCardDTO = await _characterCardService.GetAsync(id);

            if (characterCardDTO == null)
            {
                return NotFound();
            }

            return Json(characterCardDTO);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllCharacterCards()
        {
            IEnumerable<CharacterCardDTO> characterCardsDTO = await _characterCardService.BrowseAllAsync();

            if (characterCardsDTO == null)
            {
                return NotFound();
            }

            return Json(characterCardsDTO);
        }


        ////https://localhost:5001/skijumper/filter?name=alan&country=ger
        //[HttpGet("filter")]
        //public async Task<IActionResult> GetByFilter(string name, string country)
        //{
        //    IEnumerable<SkiJumperDTO> skiJumpersDTO = await _skiJumperService.BrowseAllByFilterAsync(name, country);

        //    if (skiJumpersDTO == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(skiJumpersDTO);
        //}


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacterCard([FromBody] UpdateCharacterCard characterCard, int id)
        {
            CharacterCardDTO characterCardDTO = await _characterCardService.GetAsync(id);

            if (characterCardDTO == null)
            {
                return NotFound();
            }

            if (characterCard == null)
            {
                return BadRequest();
            }

            characterCardDTO.History = characterCard.History ?? characterCardDTO.History;
            characterCardDTO.AppearanceDescription = characterCard.AppearanceDescription ?? characterCardDTO.AppearanceDescription;
            characterCardDTO.CharacterDescription = characterCard.CharacterDescription ?? characterCardDTO.CharacterDescription;

            await _characterCardService.UpdateAsync(characterCardDTO);

            return Json(characterCardDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacterCard(int id)
        {
            CharacterCardDTO characterCardDTO = await _characterCardService.GetAsync(id);

            if (characterCardDTO == null)
            {
                return NotFound();
            }

            await _characterCardService.DelAsync(characterCardDTO);

            return Ok();
        }
    }
}
