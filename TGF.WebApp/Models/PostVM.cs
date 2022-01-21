using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class PostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Short { get; set; }
        public string Content { get; set; }
    }
}
