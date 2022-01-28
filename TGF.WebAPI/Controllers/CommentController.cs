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
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateComment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            CommentDTO commentDTO = new CommentDTO()
            {
                Content = comment.Content,
                PublicationDate =comment.PublicationDate
            };

            var c = await _commentService.AddAsync(commentDTO);

            if (c == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetComment), new { id = c.Id }, c);
        }

        //https://localhost:5001/comment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            CommentDTO commentDTO = await _commentService.GetAsync(id);

            if (commentDTO == null)
            {
                return NotFound();
            }

            return Json(commentDTO);
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllComments()
        {
            IEnumerable<CommentDTO> commentsDTO = await _commentService.BrowseAllAsync();

            if (commentsDTO == null)
            {
                return NotFound();
            }

            return Json(commentsDTO);
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
        public async Task<IActionResult> UpdateComment([FromBody] UpdateComment comment, int id)
        {
            CommentDTO commentDTO = await _commentService.GetAsync(id);

            if (commentDTO == null)
            {
                return NotFound();
            }

            if (comment == null)
            {
                return BadRequest();
            }

            commentDTO.Content = comment.Content ?? commentDTO.Content;
            commentDTO.PublicationDate = comment.PublicationDate;

            await _commentService.UpdateAsync(commentDTO);

            return Json(commentDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            CommentDTO commentDTO = await _commentService.GetAsync(id);

            if (commentDTO == null)
            {
                return NotFound();
            }

            await _commentService.DelAsync(commentDTO);

            return Ok();
        }
    }
}
