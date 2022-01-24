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
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PostDTO> AddAsync(PostDTO post)
        {
            var p = await _postRepository.AddAsync(ToDomain(post));
            return p != null ? ToDTO(p) : null;
        }

        public async Task<PostDTO> GetAsync(int id)
        {
            var post = await _postRepository.GetAsync(id);

            if (post == null)
            {
                return null;
            }

            return ToDTO(post);
        }

        public async Task<IEnumerable<PostDTO>> BrowseAllAsync()
        {
            var posts = await _postRepository.BrowseAllAsync();

            if (posts == null)
            {
                return null;
            }

            return posts.Select(p => ToDTO(p));
        }

        public async Task UpdateAsync(PostDTO postDTO)
        {
            if (postDTO != null)
            {
                await _postRepository.UpdateAsync(ToDomain(postDTO));
            }
        }

        public async Task DelAsync(PostDTO postDTO)
        {
            if (postDTO != null)
            {
                await _postRepository.DelAsync(ToDomain(postDTO));
            }
        }

        private PostDTO ToDTO(Post p)
        {
            return new PostDTO()
            {
                Id = p.Id,
                Title = p.Title,
                PublicationDate = p.PublicationDate,
                Content = p.Content,
                Annotation = p.Annotation,
                ProfileId = p.ProfileId,
                CharacterId = p.CharacterId ?? 0,
                StoryId = p.StoryId ?? 0,
                Profile = p.Profile != null ? new ProfileDTO()
                {
                    Id = p.Profile.Id,
                    Name = p.Profile.Name,
                    UserID = p.Profile.UserId,
                } : null,
                Character = p.Character != null ? new CharacterDTO()
                {
                    Id = p.Character.Id,
                    Name = p.Character.Name,
                } : null
            };
        }

        private Post ToDomain(PostDTO pDTO)
        {
            return new Post()
            {
                Id = pDTO.Id,
                Title = pDTO.Title,
                Content = pDTO.Content,
                Annotation = pDTO.Annotation,
                ProfileId = pDTO.ProfileId,
                CharacterId = pDTO.CharacterId == 0 ? null : pDTO.CharacterId,
                StoryId = pDTO.StoryId == 0 ? null : pDTO.StoryId
            };
        }
    }
}
