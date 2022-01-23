using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.Commands
{
    public class CreateCharacter
    {
        public string Name { get; set; }

        //public CharacterCard CharacterCard { get; set; }

        //public ICollection<Story> Stories { get; set; }
        //public ICollection<Post> Posts { get; set; }

        public int ProfileId { get; set; }
       // public Profile Profile { get; set; }
    }
}
