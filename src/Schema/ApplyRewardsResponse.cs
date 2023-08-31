using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ARR", Namespace = "")]
[Serializable]
public class ApplyRewardsResponse {
	[XmlElement(ElementName = "ST", IsNullable = false)]
	public Status Status { get; set; }

	[XmlElement(ElementName = "UID", IsNullable = false)]
	public Guid UserID { get; set; }

	[XmlElement(ElementName = "ARS", IsNullable = false)]
	public AchievementReward[] AchievementRewards { get; set; }

	[XmlElement(ElementName = "RISM", IsNullable = true)]
	public UserItemStatsMap RewardedItemStatsMap { get; set; }

	[XmlElement(ElementName = "CIR", IsNullable = true)]
	public CommonInventoryResponse CommonInventoryResponse { get; set; }
}
