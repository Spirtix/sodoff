using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ATSRQ", Namespace = "")]
[Serializable]
public class AchievementTaskSetRequest
{
	[XmlElement(ElementName = "as")]
	public AchievementTask[] AchievementTaskSet { get; set; }
}
