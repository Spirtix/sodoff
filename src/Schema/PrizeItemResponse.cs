using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "pir", Namespace = "")]
[Serializable]
public class PrizeItemResponse
{
	[XmlElement(ElementName = "i")]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "pi")]
	public int PrizeItemID { get; set; }

	[XmlElement(ElementName = "pis", IsNullable = true)]
	public List<ItemData> MysteryPrizeItems { get; set; }
}
