using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "CIRBI", Namespace = "")]
[Serializable]
public class CommonInventoryResponseRewardBinItem : CommonInventoryResponseItem {
	[XmlElement(ElementName = "UISMID", IsNullable = false)]
	public int UserItemStatsMapID { get; set; }
}
