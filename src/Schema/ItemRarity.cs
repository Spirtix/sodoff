using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IR")]
[Serializable]
public enum ItemRarity
{
	[XmlEnum("0")]
	NonBattleCommon,

	[XmlEnum("1")]
	Common,
	
	[XmlEnum("2")]
	Rare,
	
	[XmlEnum("3")]
	Epic,
	
	[XmlEnum("4")]
	Legendary
}
