using System.Xml.Serialization;

namespace sodoff.Schema;

// Token: 0x02000472 RID: 1138
[XmlRoot(ElementName = "AvatarDisplayData", Namespace = "", IsNullable = false)]
[Serializable]
public class AvatarDisplayData
{
	[XmlElement(ElementName = "AvatarData", IsNullable = true)]
	public AvatarData AvatarData;

	[XmlElement(Namespace = "http://api.jumpstart.com/")]
	public UserInfo UserInfo;

	[XmlElement(ElementName = "UserSubscriptionInfo")]
	public UserSubscriptionInfo UserSubscriptionInfo;

	public UserAchievementInfo AchievementInfo;

	[XmlElement(ElementName = "Achievements")]
	public UserAchievementInfo[] Achievements;

	[XmlElement(ElementName = "RewardMultipliers", IsNullable = true)]
	public RewardMultiplier[] RewardMultipliers;

	public int RankID;
}
