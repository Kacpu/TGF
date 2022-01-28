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
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter([FromBody] CreateCharacter character)
        {
            if (character == null)
            {
                return BadRequest();
            }

            CharacterDTO characterDTO = new CharacterDTO()
            {
                Name = character.Name,
                ProfileId = character.ProfileId
            };

            var c = await _characterService.AddAsync(characterDTO);

            if (c == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetCharacter), new { id = c.Id }, c);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacter(int id)
        {
            CharacterDTO characterDTO = await _characterService.GetAsync(id);

            if (characterDTO == null)
            {
                return NotFound();
            }

            return Json(characterDTO);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllCharacters()
        {
            IEnumerable<CharacterDTO> charactersDTO = await _characterService.BrowseAllAsync();

            if (charactersDTO == null)
            {
                return NotFound();
            }

            return Json(charactersDTO);
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
        public async Task<IActionResult> UpdateCharacter([FromBody] UpdateCharacter character, int id)
        {
            CharacterDTO characterDTO = await _characterService.GetAsync(id);

            if (characterDTO == null)
            {
                return NotFound();
            }

            if (character == null)
            {
                return BadRequest();
            }

            characterDTO.Name = character.Name ?? characterDTO.Name;

            await _characterService.UpdateAsync(characterDTO);

            return Json(characterDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            CharacterDTO characterDTO = await _characterService.GetAsync(id);

            if (characterDTO == null)
            {
                return NotFound();
            }

            await _characterService.DelAsync(characterDTO);

            return Ok();
        }
    }
}
