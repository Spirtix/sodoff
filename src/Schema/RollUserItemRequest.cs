using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RUIR", Namespace = "")]
[Serializable]
public class RollUserItemRequest
{
	[XmlElement(ElementName = "CID", IsNullable = false)]
	public int ContainerID { get; set; }

	[XmlElement(ElementName = "UIID", IsNullable = false)]
	public int UserInventoryID { get; set; }

	[XmlElement(ElementName = "ISN", IsNullable = true)]
	public string[] ItemStatNames { get; set; }

	[XmlElement(ElementName = "CIR", IsNullable = false)]
	public CommonInventoryRequest[] InventoryItems { get; set; }
}
