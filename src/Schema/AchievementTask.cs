using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ATS", Namespace = "")]
[Serializable]
public class AchievementTask
{
	[XmlElement(ElementName = "oid")]
	public string OwnerID;

	[XmlElement(ElementName = "tid")]
	public int TaskID;

	[XmlElement(ElementName = "aiid")]
	public int AchievementInfoID;

	[XmlElement(ElementName = "rid")]
	public string RelatedID;

	[XmlElement(ElementName = "pts")]
	public int Points;

	[XmlElement(ElementName = "etid")]
	public int EntityTypeID;
}
