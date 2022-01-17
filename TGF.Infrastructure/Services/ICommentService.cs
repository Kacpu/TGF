using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface ICommentService
    {
        Task<CommentDTO> AddAsync(CommentDTO commentDTO);
        Task<CommentDTO> GetAsync(int id);
        Task<IEnumerable<CommentDTO>> BrowseAllAsync();
        Task UpdateAsync(CommentDTO commentDTO);
        Task DelAsync(CommentDTO commentDTO);
    }
}
