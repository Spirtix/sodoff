using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "CIRS", Namespace = "")]
[Serializable]
public class CommonInventoryResponse
{
	[XmlElement(ElementName = "pir", IsNullable = true)]
	public List<PrizeItemResponse> PrizeItems { get; set; }

	[XmlElement(ElementName = "s")]
	public bool Success;

	[XmlElement(ElementName = "cids")]
	public CommonInventoryResponseItem[] CommonInventoryIDs;

	[XmlElement(ElementName = "ugc", IsNullable = true)]
	public UserGameCurrency UserGameCurrency;
}
