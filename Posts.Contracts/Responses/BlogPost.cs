using System;
using System.Runtime.Serialization;

namespace Posts.Contracts.Responses
{
    [DataContract]
    public class BlogPost
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }

        [DataMember]
        public string Slug { get; set; }

        [DataMember]
        public string TagList { get; set; }
    }
}
