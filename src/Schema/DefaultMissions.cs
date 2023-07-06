using System.Xml.Serialization;

namespace sodoff.Schema;

// NOTE: This schema is NOT used by the client
// This is a schema specific to the sodoff server
[XmlRoot(ElementName = "DefaultMissions", Namespace = "")]
[Serializable]
public class DefaultMissions
{
	[XmlArray(ElementName = "Active")]
	[XmlArrayItem("id")]
	public int[] Active { get; set; }

    [XmlArray(ElementName = "Upcoming")]
	[XmlArrayItem("id")]
	public int[] Upcoming { get; set; }
}
