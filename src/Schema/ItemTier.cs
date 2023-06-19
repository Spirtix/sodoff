using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IT")]
[Serializable]
public enum ItemTier
{
	[XmlEnum("1")]
	Tier1 = 1,
	
	[XmlEnum("2")]
	Tier2,
	
	[XmlEnum("3")]
	Tier3,
	
	[XmlEnum("4")]
	Tier4
}
