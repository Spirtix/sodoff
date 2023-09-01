using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PRIREQ", IsNullable = true)]
[Serializable]
public class ProcessRewardedItemsRequest {
	[XmlElement(ElementName = "IATM", IsNullable = false)]
	public ItemActionTypeMap[] ItemsActionMap { get; set; }
}
