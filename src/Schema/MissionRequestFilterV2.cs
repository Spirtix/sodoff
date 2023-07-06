using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RequestFilter", Namespace = "")]
[Serializable]
public class MissionRequestFilterV2
{
	[XmlElement(ElementName = "MGID")]
	public int[] MissionGroupIDs { get; set; }

	[XmlElement(ElementName = "MP")]
	public List<MissionPair> MissionPair { get; set; }

	[XmlElement(ElementName = "TID")]
	public int[] TaskIDList { get; set; }

	[XmlElement(ElementName = "PID")]
	public int? ProductID { get; set; }

	[XmlElement(ElementName = "S")]
	public int? SearchDepth { get; set; }

	[XmlElement(ElementName = "GCM")]
	public bool? GetCompletedMission { get; set; }

	[XmlElement(ElementName = "PGID")]
	public int ProductGroupID;
}
