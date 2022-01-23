using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class CharacterCardVM
    {
        public int Id { get; set; }
        public string History { get; set; }
        public string AppearanceDescription { get; set; }
        public string CharacterDescription { get; set; }

        public int CharacterId { get; set; }
    }
}
