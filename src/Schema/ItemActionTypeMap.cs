using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IATM", IsNullable = true)]
[Serializable]
public class ItemActionTypeMap {
	[XmlElement(ElementName = "ID", IsNullable = false)]
	public int ID { get; set; }

	[XmlElement(ElementName = "IM", IsNullable = false)]
	public int InventoryMax { get; set; }

	[XmlElement(ElementName = "IU", IsNullable = false)]
	public int ItemUses { get; set; }

	[XmlElement(ElementName = "ACT", IsNullable = false)]
	public ActionType Action { get; set; }
}
