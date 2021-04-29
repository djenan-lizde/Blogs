using Dapper;
using Posts.Contracts.Requests;
using Posts.Contracts.Responses;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Posts.Repository
{
    public interface IBlogPostRepository
    {
        BlogPost GetBySlug(string slug);
        IEnumerable<string> GetByTag(string tag);
        BlogPost Insert(CreateBlogPost entity, string slug);
        void Update(BlogPost entity, string slugFilter);
        void Delete(string slug);
    }
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly string _connectionString;
        public BlogPostRepository(
            string connectionString
            )
        {
            _connectionString = connectionString;
        }
        public IEnumerable<string> GetByTag(string tag)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                return cn.Query<string>(@"SELECT BP.Slug FROM BlogPosts AS BP JOIN BlogPostsTags AS BPT ON BP.Id = BPT.BlogPostId
JOIN Tags as T on BPT.TagId = T.Id
where T.TagName = @tag", new { tag });

            }
        }
        public BlogPost GetBySlug(string slug)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                return cn.QueryFirstOrDefault<BlogPost>(@"SELECT B.*, 
(
SELECT STRING_AGG(T.TagName, ', ')
FROM BlogPostsTags AS BPT JOIN Tags AS T ON BPT.TagId = T.Id 
WHERE B.Id = BPT.BlogPostId
) AS TagList
FROM BlogPosts AS B 
WHERE B.Slug = @slug", new { slug });
            }
        }
        public BlogPost Insert(CreateBlogPost entity, string slug)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Execute(@"INSERT INTO BlogPosts VALUES (@title, @description, @body, GETDATE(), null, @slug)",
                    new { entity.Title, entity.Description, entity.Body, slug });

                var blog = cn.QueryFirstOrDefault<BlogPost>(@"SELECT * FROM BlogPosts WHERE Slug = @slug", new { slug });

                foreach (var tag in entity.TagList)
                {
                    var tagName = cn.QueryFirstOrDefault<Tag>(@"SELECT * FROM Tags WHERE TagName = @tag", new { tag });
                    if (tagName == null)
                    {
                        cn.Execute(@"INSERT INTO Tags VALUES (@tag)", new { tag });
                        tagName = cn.QueryFirstOrDefault<Tag>(@"SELECT * FROM Tags WHERE TagName = @tag", new { tag });
                    }
                    cn.Execute(@"INSERT INTO BlogPostsTags VALUES(@tagId, @blogId)", new { tagId = tagName.Id, blogId = blog.Id });
                }

                return blog;
            }
        }
        public void Update(BlogPost entity, string slugFilter)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Execute(@"UPDATE BlogPosts
SET Title = @title, Description = @description, Body = @body, UpdatedAt = GETDATE(), Slug = @slug
WHERE Slug = @slugFilter", new { slugFilter, entity.Slug, entity.Description, entity.Title, entity.Body });
            }
        }
        public void Delete(string slug)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Execute(@"DELETE BlogPostsTags 
FROM BlogPostsTags AS BPT JOIN BlogPosts AS B
ON BPT.BlogPostId = B.Id
WHERE B.Slug = @slug", new { slug });

                cn.Execute(@"DELETE BlogPosts WHERE Slug = @slug", new { slug });
            }
        }
    }
}
