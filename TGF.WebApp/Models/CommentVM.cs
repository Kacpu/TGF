using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class CommentVM
    {
        public int Id { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Content { get; set; }
    }
}
