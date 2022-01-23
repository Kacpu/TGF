using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Infrastructure.DTO
{
    public class CharacterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CharacterCardDTO CharacterCard { get; set; }

        public ICollection<StoryDTO> Stories { get; set; }
        //public ICollection<PostDTO> Posts { get; set; }

        public int ProfileId { get; set; }
        public ProfileDTO Profile { get; set; }
    }
}
