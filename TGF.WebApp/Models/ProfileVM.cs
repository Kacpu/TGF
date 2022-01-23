using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGF.WebApp.Models
{
    public class ProfileVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastSeen { get; set; }

        public ICollection<CharacterVM> Characters { get; set; }

        [Required(ErrorMessage = "ID użytkownika jest wymaganae!")]
        public string UserId { get; set; }

        // public ICollection<Post> Posts { get; set; }
    }
}
