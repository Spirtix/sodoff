using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ISC", Namespace = "", IsNullable = true)]
[Serializable]
public class ItemSaleConfig
{
	[XmlElement(ElementName = "IID", IsNullable = true)]
	public int? ItemID { get; set; }

	[XmlElement(ElementName = "CID", IsNullable = true)]
	public int? CategoryID { get; set; }

	[XmlElement(ElementName = "RID", IsNullable = true)]
	public int? RarityID { get; set; }

	[XmlElement(ElementName = "QTY", IsNullable = false)]
	public int Quantity { get; set; }

	[XmlElement(ElementName = "RIID", IsNullable = false)]
	public int RewardItemID { get; set; }
}
