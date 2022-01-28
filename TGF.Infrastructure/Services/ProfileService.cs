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
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IEmailSender _emailSender;

        public ProfileService(IProfileRepository profileRepository, IAdminRepository adminRepository,IEmailSender emailSender)
        {
            _profileRepository = profileRepository;
            _adminRepository = adminRepository;
            _emailSender = emailSender;
        }

        public async Task<ProfileDTO> AddAsync(ProfileDTO profile)
        {
            var p = await _profileRepository.AddAsync(await ToDomain(profile));

            if (p != null)
            {
                foreach(var admin in await _adminRepository.GetAllAsync())
                {
                    await _emailSender.SendEmailAsync(admin.Email, "TGF App - new user",
                        $"A new user has been registered:<br>Username: {p.AppUser.UserName}");
                }
                return ToDTO(p);
            }
            else
            {
                return null;
            }
        }

        public async Task<ProfileDTO> GetAsync(int id)
        {
            var profile = await _profileRepository.GetAsync(id);

            if (profile == null)
            {
                return null;
            }

            return ToDTO(profile);
        }

        public async Task<IEnumerable<ProfileDTO>> BrowseAllAsync()
        {
            var profiles = await _profileRepository.BrowseAllAsync();

            if (profiles == null)
            {
                return null;
            }

            return profiles.Select(p => ToDTO(p));
        }
        
        public async Task<ProfileDTO> FindByUsername(string username)
        {
            var profile = await _profileRepository.FindByUsername(username);

            if (profile == null)
            {
                return null;
            }

            return ToDTO(profile);
        }

        public async Task UpdateAsync(ProfileDTO profileDTO)
        {
            if (profileDTO != null)
            {
                await _profileRepository.UpdateAsync(await ToDomain(profileDTO));
            }
        }

        public async Task DelAsync(ProfileDTO profileDTO)
        {
            if (profileDTO != null)
            {
                await _profileRepository.DelAsync(await ToDomain(profileDTO));
            }
        }

        private ProfileDTO ToDTO(Profile p)
        {
            ICollection<CharacterDTO> charactersDTO = new List<CharacterDTO>();
            if(p.Characters != null)
            {
                foreach (var ch in p.Characters)
                {
                    ICollection<StoryDTO> storiesDTO = new List<StoryDTO>();
                    if (ch.Stories != null)
                    {
                        foreach (var story in ch.Stories)
                        {
                            storiesDTO.Add(new StoryDTO
                            {
                                Id = story.Id,
                            });
                        }
                    }

                    charactersDTO.Add(new CharacterDTO
                    {
                        Id = ch.Id,
                        Name = ch.Name,
                        Stories = storiesDTO
                    });
                }
            }

            return new ProfileDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                LastSeen = p.LastSeen,

                UserID = p.UserId,

                Characters = charactersDTO
            };
        }

        private async Task<Profile> ToDomain(ProfileDTO pDTO)
        {
            return new Profile()
            {
                Id = pDTO.Id,
                Name = pDTO.Name,
                Description = pDTO.Description,
                LastSeen = pDTO.LastSeen,
                UserId = pDTO.UserID

                // Trainer = await _skiJumperRepository.GetTrainerAsync(pDTO.TrainerId)
            };
        }
    }
}
