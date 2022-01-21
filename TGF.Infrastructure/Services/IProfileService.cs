using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Infrastructure.DTO;

namespace TGF.Infrastructure.Services
{
    public interface IProfileService
    {
        Task<ProfileDTO> AddAsync(ProfileDTO profileDTO);
        Task<ProfileDTO> GetAsync(int id);
        Task<IEnumerable<ProfileDTO>> BrowseAllAsync();
        Task<ProfileDTO> FindByUsername(string username);
        Task UpdateAsync(ProfileDTO profileDTO);
        Task DelAsync(ProfileDTO profileDTO);
    }
}
