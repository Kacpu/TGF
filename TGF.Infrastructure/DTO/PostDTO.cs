using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Content { get; set; }
        public string Annotation { get; set; }

        //public ICollection<CommentDTO> Comments { get; set; }

        public int ProfileId { get; set; }
        public ProfileDTO Profile { get; set; }

        public int? CharacterId { get; set; }
        public CharacterDTO Character { get; set; }

        public int? StoryId { get; set; }
        //public StoryDTO Story { get; set; }
    }
}
