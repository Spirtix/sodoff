using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "SIR", Namespace = "")]
[Serializable]
public class SellItemsRequest
{
	[XmlElement(ElementName = "CID", IsNullable = false)]
	public int ContainerID { get; set; }

	[XmlElement(ElementName = "UICDS", IsNullable = false)]
	public int[] UserInventoryCommonIDs { get; set; }
}
