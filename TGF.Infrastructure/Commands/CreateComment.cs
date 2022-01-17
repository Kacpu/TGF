using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.Commands
{
    public class CreateComment
    {
        public DateTime PublicationDate { get; set; }
        public string Content { get; set; }
    }
}
