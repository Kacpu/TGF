using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Core.Domain
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Content { get; set; }
        public string Annotation { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int? CharacterId { get; set; }
        public Character Character { get; set; }

        public int? StoryId { get; set; }
        public Story Story { get; set; }
    }
}
