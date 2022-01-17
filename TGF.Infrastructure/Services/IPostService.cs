using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface IPostService
    {
        Task<PostDTO> AddAsync(PostDTO postDTO);
        Task<PostDTO> GetAsync(int id);
        Task<IEnumerable<PostDTO>> BrowseAllAsync();
        Task UpdateAsync(PostDTO postDTO);
        Task DelAsync(PostDTO postDTO);
    }
}
