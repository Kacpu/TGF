using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Core.Domain
{
    public class CharacterCard
    {
        public int Id { get; set; }
        public string History { get; set; }
        public string AppearanceDescription { get; set; }
        public string CharacterDescription { get; set; }

        public int? CharacterId { get; set; }//?
        public Character Character { get; set; }
    }
}
