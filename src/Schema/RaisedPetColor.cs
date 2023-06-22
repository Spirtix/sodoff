using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RPC", Namespace = "")]
[Serializable]
public class RaisedPetColor
{
	[XmlElement(ElementName = "o")]
	public int Order;

	[XmlElement(ElementName = "r")]
	public float Red;

	[XmlElement(ElementName = "g")]
	public float Green;

	[XmlElement(ElementName = "b")]
	public float Blue;
}
