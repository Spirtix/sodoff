using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ACT")]
[Serializable]
public enum ActionType {
	[XmlEnum("1")]
	MoveToInventory = 1,
	
	[XmlEnum("2")]
	SellInventoryItem = 2,
	
	[XmlEnum("3")]
	SellRewardBinItem = 3
}
