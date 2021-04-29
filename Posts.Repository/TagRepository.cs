using Dapper;
using Posts.Contracts.Responses;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Posts.Repository
{
    public interface ITagRepository 
    {
        IEnumerable<Tag> Get();
    }
    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;
        public TagRepository(
            string connectionString
            )
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Tag> Get()
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                return cn.Query<Tag>(@"SELECT * FROM Tags");
            }
        }
    }
}
