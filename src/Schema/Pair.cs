using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Pair", Namespace = "")]
[Serializable]
public class Pair
{
	[XmlElement(ElementName = "PairKey")]
	public string PairKey;
	
	[XmlElement(ElementName = "PairValue")]
	public string PairValue;
	
	[XmlElement(ElementName = "UpdateDate")]
	public DateTime UpdateDate;
}