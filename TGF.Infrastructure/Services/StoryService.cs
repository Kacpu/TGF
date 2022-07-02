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
            var s = await _storyRepository.AddAsync(ToDomain(story));
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
                await _storyRepository.UpdateAsync(ToDomain(storyDTO));
            }
        }

        public async Task DelAsync(StoryDTO storyDTO)
        {
            if (storyDTO != null)
            {
                await _storyRepository.DelAsync(ToDomain(storyDTO));
            }
        }

        private StoryDTO ToDTO(Story s)
        {
            ICollection<CharacterDTO> charactersDTO = new List<CharacterDTO>();
            if (s.Characters != null)
            {
                foreach (var ch in s.Characters)
                {
                    charactersDTO.Add(new CharacterDTO
                    {
                        Id = ch.Id,
                        Name = ch.Name,
                        Profile = ch.Profile != null ? new ProfileDTO()
                        {
                            Id = ch.Profile.Id,
                            Name = ch.Profile.Name,
                            UserID = ch.Profile.UserId
                        } : null,
                    });
                }
            }
            ICollection<PostDTO> postsDTO = new List<PostDTO>();
            if (s.Posts != null)
            {
                foreach (var p in s.Posts)
                {
                    postsDTO.Add(new PostDTO
                    {
                        Id = p.Id,
                        Title = p.Title,
                        PublicationDate = p.PublicationDate,
                        Content = p.Content,
                        Annotation = p.Annotation,
                        Character = p.Character != null ? new CharacterDTO()
                        {
                            Id = p.Character.Id,
                            Name = p.Character.Name,
                            Profile = p.Character.Profile != null ? new ProfileDTO()
                            {
                                Id = p.Character.Profile.Id,
                                Name = p.Character.Profile.Name,
                                UserID = p.Character.Profile.UserId,
                            } : null
                        } : null
                    });
                }
            }
            return new StoryDTO()
            {
                Id = s.Id,
                Title = s.Title,
                CreationDate = s.CreationDate,
                Characters = charactersDTO,
                Posts = postsDTO
            };
        }

        private Story ToDomain(StoryDTO sDTO)
        {
            return new Story()
            {
                Id = sDTO.Id,
                Title = sDTO.Title
            };
        }
    }
}
