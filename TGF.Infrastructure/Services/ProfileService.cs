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

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<ProfileDTO> AddAsync(ProfileDTO profile)
        {
            var p = await _profileRepository.AddAsync(await ToDomain(profile));
            return p != null ? ToDTO(p) : null;
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
            return new ProfileDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                LastSeen = p.LastSeen,
                
                UserID = p.UserId

                // TrainerId = p.Trainer != null ? p.Trainer.Id : -1
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
