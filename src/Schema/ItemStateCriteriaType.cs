using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public enum ItemStateCriteriaType
{
	[XmlEnum("1")]
	Length = 1,
	
	[XmlEnum("2")]
	ConsumableItem,
	
	[XmlEnum("3")]
	ReplenishableItem,
	
	[XmlEnum("4")]
	SpeedUpItem,
	
	[XmlEnum("5")]
	StateExpiry
}
