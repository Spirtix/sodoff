using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "IRO", Namespace = "")]
[Serializable]
public class ItemDataRollover
{
	[XmlElement(ElementName = "d")]
	public string DialogName;

	[XmlElement(ElementName = "b")]
	public string Bundle;
}
