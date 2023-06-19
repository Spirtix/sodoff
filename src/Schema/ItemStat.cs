using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IS", Namespace = "")]
[Serializable]
public class ItemStat
{
	[XmlElement(ElementName = "ID")]
	public int ItemStatID { get; set; }

	[XmlElement(ElementName = "N")]
	public string Name { get; set; }

	[XmlElement(ElementName = "V")]
	public string Value { get; set; }

	[XmlElement(ElementName = "DTI")]
	public DataTypeInfo DataType { get; set; }
}
