using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateCriteriaSpeedUpItem", Namespace = "")]
public class ItemStateCriteriaSpeedUpItem : ItemStateCriteria
{
	[XmlElement(ElementName = "ItemID")]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "ConsumeUses")]
	public bool ConsumeUses { get; set; }

	[XmlElement(ElementName = "Amount")]
	public int Amount { get; set; }

	[XmlElement(ElementName = "ChangeState")]
	public bool ChangeState { get; set; }

	[XmlElement(ElementName = "EndStateID")]
	public int EndStateID { get; set; }

	[XmlElement(ElementName = "SpeedUpCapacity")]
	public int SpeedUpCapacity { get; set; }

	[XmlElement(ElementName = "SpeedUpUses")]
	public bool SpeedUpUses { get; set; }
}
