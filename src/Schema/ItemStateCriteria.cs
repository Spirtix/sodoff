using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateCriteria", Namespace = "")]
[XmlInclude(typeof(ItemStateCriteriaLength))]
[XmlInclude(typeof(ItemStateCriteriaConsumable))]
[XmlInclude(typeof(ItemStateCriteriaReplenishable))]
[XmlInclude(typeof(ItemStateCriteriaOverride))]
[XmlInclude(typeof(ItemStateCriteriaSpeedUpItem))]
[XmlInclude(typeof(ItemStateCriteriaExpiry))]
[Serializable]
public class ItemStateCriteria
{
	[XmlElement(ElementName = "Type")]
	public ItemStateCriteriaType Type;
}
