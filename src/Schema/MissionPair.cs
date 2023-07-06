using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MP", Namespace = "")]
[Serializable]
public class MissionPair
{
	[XmlElement(ElementName = "MID")]
	public int? MissionID { get; set; }

	[XmlElement(ElementName = "VID")]
	public int? VersionID { get; set; }
}
