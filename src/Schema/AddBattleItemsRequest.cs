using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ABIR", Namespace = "")]
[Serializable]
public class AddBattleItemsRequest {
	[XmlElement(ElementName = "BITM", IsNullable = false)]
	public List<BattleItemTierMap> BattleItemTierMaps { get; set; }
}
