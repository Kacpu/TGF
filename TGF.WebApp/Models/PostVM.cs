using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class PostVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany!")]
        public string Title { get; set; }

        public DateTime PublicationDate { get; set; }
        public string Short { get; set; }
        public string Content { get; set; }

        //public ICollection<CommentVM> Comments { get; set; }

        public int ProfileId { get; set; }
        public ProfileVM Profile { get; set; }

        [Required(ErrorMessage = "Charakter jest wymagany!")]
        public int? CharacterId { get; set; }

        public CharacterVM Character { get; set; }

        public int? StoryId { get; set; }
        //public StoryVM Story { get; set; }
    }
}

