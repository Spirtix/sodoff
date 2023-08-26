using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "FIR", Namespace = "")]
[Serializable]
public class FuseItemsRequest
{
	[XmlElement(ElementName = "BPINVID", IsNullable = true)]
	public int? BluePrintInventoryID { get; set; }

	[XmlElement(ElementName = "BPIID", IsNullable = true)]
	public int? BluePrintItemID { get; set; }

	[XmlElement(ElementName = "DIIM", IsNullable = true)]
	public List<DeductibleItemInventoryMap> DeductibleItemInventoryMaps { get; set; }

	[XmlElement(ElementName = "BPFIM", IsNullable = false)]
	public List<BluePrintFuseItemMap> BluePrintFuseItemMaps { get; set; }

	[XmlElement(ElementName = "LOC", IsNullable = true)]
	public string Locale { get; set; }

	[XmlElement(ElementName = "AGN", IsNullable = false)]
	public Gender AvatarGender { get; set; }
}
