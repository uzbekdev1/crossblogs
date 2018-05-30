using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crossblog.Domain
{
    public class Comment : BaseEntity
    {

        [ForeignKey("Article")]
        public int? ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; }

        [Required]
        [StringLength(32000)]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; } 
    }
}