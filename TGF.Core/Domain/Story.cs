﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Core.Domain
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}
