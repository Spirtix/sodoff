using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PT", Namespace = "")]
[Serializable]
public class ProfileTag
{
	[XmlElement(ElementName = "ID")]
	public int TagID;

	[XmlElement(ElementName = "NA", IsNullable = true)]
	public string TagName;

	[XmlElement(ElementName = "VAL", IsNullable = true)]
	public int? Value;
}
