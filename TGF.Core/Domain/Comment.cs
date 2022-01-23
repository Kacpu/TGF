using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Core.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public string Content { get; set; }

        public int? PostId { get; set; }//?
        public Post Post { get; set; }
    }
}
