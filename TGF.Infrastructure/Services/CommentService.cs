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
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDTO> AddAsync(CommentDTO commentDTO)
        {
            var c = await _commentRepository.AddAsync(await ToDomain(commentDTO));
            return c != null ? ToDTO(c) : null;
        }

        public async Task<CommentDTO> GetAsync(int id)
        {
            var comment = await _commentRepository.GetAsync(id);

            if (comment == null)
            {
                return null;
            }

            return ToDTO(comment);
        }

        public async Task<IEnumerable<CommentDTO>> BrowseAllAsync()
        {
            var comments = await _commentRepository.BrowseAllAsync();

            if (comments == null)
            {
                return null;
            }

            return comments.Select(c => ToDTO(c));
        }

        public async Task UpdateAsync(CommentDTO commentDTO)
        {
            if (commentDTO != null)
            {
                await _commentRepository.UpdateAsync(await ToDomain(commentDTO));
            }
        }

        public async Task DelAsync(CommentDTO commentDTO)
        {
            if (commentDTO != null)
            {
                await _commentRepository.DelAsync(await ToDomain(commentDTO));
            }
        }

        private CommentDTO ToDTO(Comment c)
        {
            return new CommentDTO()
            {
                Id = c.Id,
                PublicationDate = c.PublicationDate,
                Content = c.Content
            };
        }

        private async Task<Comment> ToDomain(CommentDTO cDTO)
        {
            return new Comment()
            {
                Id = cDTO.Id,
                PublicationDate = cDTO.PublicationDate,
                Content = cDTO.Content
            };
        }
    }
}
