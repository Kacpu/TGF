using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.DTO
{
    public class StoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<PostDTO> Posts { get; set; }
        public ICollection<CharacterDTO> Characters { get; set; }
    }
}
