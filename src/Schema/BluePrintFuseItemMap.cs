using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "BPFIM", Namespace = "")]
[Serializable]
public class BluePrintFuseItemMap {
	[XmlElement(ElementName = "BPSID", IsNullable = false)]
	public int BluePrintSpecID { get; set; }

	[XmlElement(ElementName = "UID", IsNullable = false)]
	public int UserInventoryID { get; set; }
}
