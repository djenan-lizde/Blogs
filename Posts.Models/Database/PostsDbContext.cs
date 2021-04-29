using Microsoft.EntityFrameworkCore;
using Posts.Models.Models;

namespace Posts.Models.Database
{
    public class PostsDbContext : DbContext
    {
        public PostsDbContext(DbContextOptions<PostsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<BlogPostTags> BlogPostsTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
    }
}
