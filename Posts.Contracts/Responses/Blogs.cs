using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Posts.Contracts.Responses
{
    [DataContract]
    public class Blogs
    {
        [DataMember]
        public List<BlogPost> BlogPosts { get; set; }

        [DataMember]
        public int PostsCount { get; set; }
    }
}
