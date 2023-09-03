using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AR", Namespace = "")]
[Serializable]
public class AchievementReward
{
	[XmlElement(ElementName = "ui", IsNullable = true)]
	public UserItemData UserItem { get; set; }

	[XmlElement(ElementName = "a")]
	public int? Amount;

	[XmlElement(ElementName = "p", IsNullable = true)]
	public AchievementPointTypes? PointTypeID;

	[XmlElement(ElementName = "ii")]
	public int ItemID;

	[XmlElement(ElementName = "i", IsNullable = true)]
	public Guid? EntityID;

	[XmlElement(ElementName = "t")]
	public int EntityTypeID;

	[XmlElement(ElementName = "r")]
	public int RewardID;

	[XmlElement(ElementName = "ai")]
	public int AchievementID;

	[XmlElement(ElementName = "amulti")]
	public bool AllowMultiple;

	[XmlElement(ElementName = "mina", IsNullable = true)]
	public int? MinAmount;

	[XmlElement(ElementName = "maxa", IsNullable = true)]
	public int? MaxAmount;

	[XmlElement(ElementName = "d", IsNullable = true)]
	public DateTime? Date;

	[XmlElement(ElementName = "cid")]
	public int CommonInventoryID;

	public AchievementReward Clone() {
		return (AchievementReward) this.MemberwiseClone();
	}
}
