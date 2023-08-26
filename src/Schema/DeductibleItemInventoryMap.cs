using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "DIIM", Namespace = "")]
[Serializable]
public class DeductibleItemInventoryMap {
	[XmlElement(ElementName = "UID", IsNullable = false)]
	public int UserInventoryID { get; set; }

	[XmlElement(ElementName = "IID", IsNullable = false)]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "QTY", IsNullable = false)]
	public int Quantity { get; set; }
}
