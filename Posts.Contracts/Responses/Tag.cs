using System.Runtime.Serialization;

namespace Posts.Contracts.Responses
{
    [DataContract]
    public class Tag
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string TagName { get; set; }
    }
}
