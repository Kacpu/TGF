using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.Commands
{
    public class CreatePost
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Annotation { get; set; }
        public int ProfileId { get; set; }
        public int? CharacterId { get; set; }
        public int? StoryId { get; set; }
    }
}
