using System;
using System.ComponentModel.DataAnnotations;

namespace crossblog.Model
{
    public class CommentModel
    {
        public int Id { get; set; }

        public int? ArticleId { get; set; }

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