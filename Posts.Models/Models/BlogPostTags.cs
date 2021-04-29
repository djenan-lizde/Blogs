using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posts.Models.Models
{
    public class BlogPostTags
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Tag))]
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        [ForeignKey(nameof(BlogPost))]
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
