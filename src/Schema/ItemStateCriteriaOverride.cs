using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateCriteriaOverride", Namespace = "")]
[Serializable]
public class ItemStateCriteriaOverride : ItemStateCriteria
{
	[XmlElement(ElementName = "ItemID")]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "ConsumeUses")]
	public bool ConsumeUses { get; set; }

	[XmlElement(ElementName = "Amount")]
	public int Amount { get; set; }
}
