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
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProfile([FromBody] CreateProfile profile)
        {
            if (profile == null)
            {
                return BadRequest();
            }

            ProfileDTO profileDTO = new ProfileDTO()
            {
                Name = profile.Name,
                Description = profile.Description,
                LastSeen = profile.LastSeen
            };

            var p = await _profileService.AddAsync(profileDTO);

            if (p == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProfile), new { id = p.Id }, p);
        }

        //https://localhost:5001/profile/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            ProfileDTO profileDTO = await _profileService.GetAsync(id);

            if (profileDTO == null)
            {
                return NotFound();
            }

            return Json(profileDTO);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllProfiles()
        {
            IEnumerable<ProfileDTO> profilesDTO = await _profileService.BrowseAllAsync();

            if (profilesDTO == null)
            {
                return NotFound();
            }

            return Json(profilesDTO);
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
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile profile, int id)
        {
            ProfileDTO profileDTO = await _profileService.GetAsync(id);

            if (profileDTO == null)
            {
                return NotFound();
            }

            if (profile == null)
            {
                return BadRequest();
            }

            profileDTO.Name = profile.Name ?? profileDTO.Name;
            profileDTO.Description = profile.Description ?? profileDTO.Description;
            profileDTO.LastSeen = profile.LastSeen;

            await _profileService.UpdateAsync(profileDTO);

            return Json(profileDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            ProfileDTO profileDTO = await _profileService.GetAsync(id);

            if (profileDTO == null)
            {
                return NotFound();
            }

            await _profileService.DelAsync(profileDTO);

            return Ok();
        }
    }
}
