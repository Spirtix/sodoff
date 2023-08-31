using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RUIRES", Namespace = "")]
[Serializable]
public class RollUserItemResponse
{
	[XmlElement(ElementName = "ST", IsNullable = false)]
	public Status Status { get; set; }

	[XmlElement(ElementName = "IS", IsNullable = false)]
	public ItemStat[] ItemStats { get; set; }
}
