using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AchievementsIdInfo", Namespace = "")]
[Serializable]
public class AchievementsIdInfo
{
	[XmlElement(ElementName = "AID")]
	public int AchievementID;
	
	[XmlElement(ElementName = "AR")]
	public AchievementReward[] AchievementReward;
}
