using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "CIRT", Namespace = "")]
[Serializable]
public class CommonInventoryRequest
{
	[XmlElement(ElementName = "uipid")]
	public int? UserItemPositionID { get; set; }

	[XmlElement(ElementName = "u")]
	public int? Uses { get; set; }

	[XmlElement(ElementName = "uia", IsNullable = true)]
	public PairData UserItemAttributes { get; set; }

	[XmlElement(ElementName = "im", IsNullable = true)]
	public int? InventoryMax { get; set; }

	[XmlElement(ElementName = "iid")]
	public int? ItemID;

	[XmlElement(ElementName = "cid")]
	public int? CommonInventoryID;

	[XmlElement(ElementName = "q")]
	public int Quantity;
}
