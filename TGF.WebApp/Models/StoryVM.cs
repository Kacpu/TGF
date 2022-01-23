using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class StoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagana!")]
        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<PostVM> Posts { get; set; }
        public ICollection<CharacterVM> Characters { get; set; }

        public int FromCharacter { get; set; }

        public ICollection<CharacterVM> OtherCharacters { get; set; }
    }
}
