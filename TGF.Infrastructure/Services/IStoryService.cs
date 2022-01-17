using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface IStoryService
    {
        Task<StoryDTO> AddAsync(StoryDTO storyDTO);
        Task<StoryDTO> GetAsync(int id);
        Task<IEnumerable<StoryDTO>> BrowseAllAsync();
        Task UpdateAsync(StoryDTO storyDTO);
        Task DelAsync(StoryDTO storyDTO);
    }
}
