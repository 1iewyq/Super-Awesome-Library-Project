using System.Runtime.Serialization;

namespace DBInterface
{
    [DataContract]
    public class IndexOutOfRangeFault
    {
        [DataMember]
        public string Issue { get; set; }
    }
}
