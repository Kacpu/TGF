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
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;

        public StoryService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<StoryDTO> AddAsync(StoryDTO story)
        {
            var s = await _storyRepository.AddAsync(await ToDomain(story));
            return s != null ? ToDTO(s) : null;
        }

        public async Task<StoryDTO> GetAsync(int id)
        {
            var story = await _storyRepository.GetAsync(id);

            if (story == null)
            {
                return null;
            }

            return ToDTO(story);
        }

        public async Task<IEnumerable<StoryDTO>> BrowseAllAsync()
        {
            var stories = await _storyRepository.BrowseAllAsync();

            if (stories == null)
            {
                return null;
            }

            return stories.Select(s => ToDTO(s));
        }

        public async Task UpdateAsync(StoryDTO storyDTO)
        {
            if (storyDTO != null)
            {
                await _storyRepository.UpdateAsync(await ToDomain(storyDTO));
            }
        }

        public async Task DelAsync(StoryDTO storyDTO)
        {
            if (storyDTO != null)
            {
                await _storyRepository.DelAsync(await ToDomain(storyDTO));
            }
        }

        private StoryDTO ToDTO(Story s)
        {
            return new StoryDTO()
            {
                Id = s.Id,
                Title = s.Title,
                CreationDate = s.CreationDate
            };
        }

        private async Task<Story> ToDomain(StoryDTO sDTO)
        {
            return new Story()
            {
                Id = sDTO.Id,
                Title = sDTO.Title,
                CreationDate = sDTO.CreationDate
            };
        }
    }
}
