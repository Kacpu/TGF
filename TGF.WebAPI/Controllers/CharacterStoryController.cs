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
    [Route("[Controller]")]
    public class CharacterStoryController : Controller
    {
        private readonly ICharacterStoryService _characterStoryService;

        public CharacterStoryController(ICharacterStoryService characterStoryService)
        {
            _characterStoryService = characterStoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacterStory([FromBody] CreateCharacterStory cs)
        {
            if (cs == null)
            {
                return BadRequest();
            }

            CharacterStoryDTO csDTO = new CharacterStoryDTO()
            {
                CharactersId = cs.CharactersId,
                StoriesId = cs.StoriesId
            };

            var res = await _characterStoryService.AddAsync(csDTO);

            if (res == null)
            {
                return BadRequest();
            }

            return Ok();
            //return CreatedAtAction(nameof(GetCharacterStory), new { cId = res.CharactersId, sId = res.StoriesId }, res);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetCharacterStory(int cId, int sId)
        //{
        //    CharacterStoryDTO csDTO = await _characterStoryService.GetAsync(cId, sId);

        //    if (csDTO == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(csDTO);
        //}

        [HttpDelete]
        public async Task<IActionResult> DeleteCharacterStory(int cId, int sId)
        {
            //CharacterStoryDTO csDTO = await _characterStoryService.GetAsync(cId, sId);

            //if (csDTO == null)
            //{
            //    return NotFound();
            //}

            //await _characterStoryService.DelAsync(csDTO);

            CharacterStoryDTO csDTO = new CharacterStoryDTO()
            {
                CharactersId = cId,
                StoriesId = sId
            };

            await _characterStoryService.DelAsync(csDTO);

            return Ok();
        }
    }
}
