using System.Runtime.Serialization;

namespace Posts.Contracts.Requests
{
    [DataContract]
    public class UpdateBlogPost
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Body { get; set; }
    }
}
