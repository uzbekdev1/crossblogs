using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crossblog.Domain
{
    public class Article : BaseEntity
    {
        [Required]
        [StringLength(120)]
        public string Title { get; set; }

        [Required]
        [StringLength(32000)]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public Article()
        {
            Comments = new HashSet<Comment>();
        }
    }
}