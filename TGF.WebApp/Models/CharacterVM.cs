using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class CharacterVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ProfileId { get; set; }

        public CharacterCardVM CharacterCard { get; set; }

        //public ICollection<Story> Stories { get; set; }
        //public ICollection<Post> Posts { get; set; }
    }
}
