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
    public class StoryController : Controller
    {
        private readonly IStoryService _storyService;

        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpPost]
        public async Task<IActionResult> AddStory([FromBody] CreateStory story)
        {
            if (story == null)
            {
                return BadRequest();
            }

            StoryDTO storyDTO = new StoryDTO()
            {
                Title = story.Title,
            };

            var s = await _storyService.AddAsync(storyDTO);

            if (s == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetStory), new { id = s.Id }, s);
        }

        //https://localhost:5001/post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStory(int id)
        {
            StoryDTO storyDTO = await _storyService.GetAsync(id);

            if (storyDTO == null)
            {
                return NotFound();
            }

            return Json(storyDTO);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllStories()
        {
            IEnumerable<StoryDTO> storiesDTO = await _storyService.BrowseAllAsync();

            if (storiesDTO == null)
            {
                return NotFound();
            }

            return Json(storiesDTO);
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
        public async Task<IActionResult> UpdateStory([FromBody] UpdateStory story, int id)
        {
            StoryDTO storyDTO = await _storyService.GetAsync(id);

            if (storyDTO == null)
            {
                return NotFound();
            }

            if (story == null)
            {
                return BadRequest();
            }

            storyDTO.Title = story.Title ?? storyDTO.Title;

            await _storyService.UpdateAsync(storyDTO);

            return Json(storyDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStory(int id)
        {
            StoryDTO storyDTO = await _storyService.GetAsync(id);

            if (storyDTO == null)
            {
                return NotFound();
            }

            await _storyService.DelAsync(storyDTO);

            return Ok();
        }
    }
}
