using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IISM", Namespace = "")]
[Serializable]
public class InventoryItemStatsMap {
	[XmlElement(ElementName = "CID", IsNullable = false)]
	public int CommonInventoryID { get; set; }

	[XmlElement(ElementName = "ITM", IsNullable = false)]
	public ItemData Item { get; set; }

	[XmlElement(ElementName = "ISM", IsNullable = false)]
	public ItemStatsMap ItemStatsMap { get; set; }
}
