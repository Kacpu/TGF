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
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] CreatePost post)
        {
            if (post == null)
            {
                return BadRequest();
            }

            PostDTO postDTO = new PostDTO()
            {
                Title = post.Title,
                Content = post.Content,
                Annotation = post.Annotation,
                ProfileId = post.ProfileId,
                CharacterId = post.CharacterId,
                StoryId = post.StoryId
            };

            var p = await _postService.AddAsync(postDTO);

            if (p == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetPost), new { id = p.Id }, p);
        }

        //https://localhost:5001/post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            PostDTO postDTO = await _postService.GetAsync(id);

            if (postDTO == null)
            {
                return NotFound();
            }

            return Json(postDTO);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllPosts()
        {
            IEnumerable<PostDTO> postsDTO = await _postService.BrowseAllAsync();

            if (postsDTO == null)
            {
                return NotFound();
            }

            return Json(postsDTO);
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
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePost post, int id)
        {
            PostDTO postDTO = await _postService.GetAsync(id);

            if (postDTO == null)
            {
                return NotFound();
            }

            if (post == null)
            {
                return BadRequest();
            }

            postDTO.Title = post.Title ?? postDTO.Title;
            postDTO.Content = post.Content ?? postDTO.Content;
            postDTO.Annotation = post.Annotation ?? postDTO.Annotation;

            await _postService.UpdateAsync(postDTO);

            return Json(postDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            PostDTO postDTO = await _postService.GetAsync(id);

            if (postDTO == null)
            {
                return NotFound();
            }

            await _postService.DelAsync(postDTO);

            return Ok();
        }
    }
}
