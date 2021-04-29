using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Posts.Contracts.Requests
{
    [DataContract]
    public class CreateBlogPost
    {
        [DataMember]
        [Required]
        public string Title { get; set; }

        [DataMember]
        [Required]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public string Body { get; set; }

        [DataMember]
        public List<string> TagList { get; set; }
    }
}
