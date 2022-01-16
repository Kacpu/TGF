using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.Commands
{
    public class UpdateProfile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastSeen { get; set; }

        // public ICollection<Character> Characters { get; set; }
        //public ICollection<Post> Posts { get; set; }
    }
}
