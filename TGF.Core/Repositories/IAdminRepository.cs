using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Core.Repositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<AppUser>> GetAllAsync();
    }
}
