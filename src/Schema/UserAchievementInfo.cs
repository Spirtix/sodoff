using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UAI", Namespace = "")]
[Serializable]
public class UserAchievementInfo
{
	[XmlElement(ElementName = "u")]
	public Guid? UserID;

	[XmlElement(ElementName = "n")]
	public string UserName;

	[XmlElement(ElementName = "a")]
	public int? AchievementPointTotal;

	[XmlElement(ElementName = "r")]
	public int RankID;

	[XmlElement(ElementName = "p")]
	public AchievementPointTypes? PointTypeID;

	[XmlElement(ElementName = "FBUID", IsNullable = true)]
	public long? FacebookUserID;
}
