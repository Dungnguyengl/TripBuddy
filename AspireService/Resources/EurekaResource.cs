using System.Xml.Serialization;

namespace AspireService.Resources
{
    [XmlRoot("application")]
    public class EurekaNode
    {
        [XmlElement("instance")]
        public List<EurekaInstance> EurekaInstances {get; set; }
    }

    public class EurekaInstance
    {
        [XmlElement("instanceId")]
        public string InstanceId {get; set; }
    }
}
