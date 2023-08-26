using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RPEM", Namespace = "")]
[Serializable]
public class RaisedPetEntityMap
{
	[XmlElement(ElementName = "RPID")]
	public int RaisedPetID { get; set; }

	[XmlElement(ElementName = "EID")]
	public Guid? EntityID { get; set; }
}
