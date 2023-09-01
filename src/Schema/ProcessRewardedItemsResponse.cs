using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PRIRES", IsNullable = true)]
[Serializable]
public class ProcessRewardedItemsResponse {
	[XmlElement(ElementName = "SII", IsNullable = true)]
	public bool? SoldInventoryItems { get; set; }

	[XmlElement(ElementName = "SRBI", IsNullable = true)]
	public bool? SoldRewardBinItems { get; set; }

	[XmlElement(ElementName = "CIRRBIS", IsNullable = true)]
	public CommonInventoryResponseRewardBinItem[] MovedRewardBinItems { get; set; }

	[XmlElement(ElementName = "CIR", IsNullable = true)]
	public CommonInventoryResponse CommonInventoryResponse { get; set; }
}
