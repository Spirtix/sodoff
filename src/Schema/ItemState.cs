using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemState", Namespace = "")]
[Serializable]
public class ItemState
{
	[XmlElement(ElementName = "ItemStateID")]
	public int ItemStateID;

	[XmlElement(ElementName = "Name")]
	public string Name;

	[XmlElement(ElementName = "Rule")]
	public ItemStateRule Rule;

	[XmlElement(ElementName = "Order")]
	public int Order;

	[XmlElement(ElementName = "AchievementID", IsNullable = true)]
	public int? AchievementID;

	[XmlElement(ElementName = "Rewards")]
	public AchievementReward[] Rewards;
}
