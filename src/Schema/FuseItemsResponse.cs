using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "FIRES", Namespace = "")]
[Serializable]
public class FuseItemsResponse
{
	[XmlElement(ElementName = "ST", IsNullable = false)]
	public Status Status { get; set; }

	[XmlElement(ElementName = "UID", IsNullable = false)]
	public Guid UserID { get; set; }

	[XmlElement(ElementName = "IISM", IsNullable = true)]
	public List<InventoryItemStatsMap> InventoryItemStatsMaps { get; set; }

	[XmlElement(ElementName = "VMSG", IsNullable = true)]
	public ValidationMessage VMsg { get; set; }
}
