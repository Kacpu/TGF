using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Core.Domain
{
    public class AppUser : IdentityUser
    {
        public Profile Profile { get; set; }
    }
}
