using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ABIRES", Namespace = "")]
[Serializable]
public class AddBattleItemsResponse {
	[XmlElement(ElementName = "ST", IsNullable = false)]
	public Status Status { get; set; }

	[XmlElement(ElementName = "IISM", IsNullable = true)]
	public List<InventoryItemStatsMap> InventoryItemStatsMaps { get; set; }
}
