using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGF.Core.Domain;

namespace TGF.Infrastructure.DTO
{
    public class ProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastSeen { get; set; }

        public string UserID { get; set; }

        public ICollection<CharacterDTO> Characters { get; set; }
       // public ICollection<Post> Posts { get; set; }
    }
}
