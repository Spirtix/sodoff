using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RPGS", Namespace = "")]
[Serializable]
public class RaisedPetGrowthState
{
	[XmlElement(ElementName = "id")]
	public int GrowthStateID;

	[XmlElement(ElementName = "n")]
	public string Name;

	[XmlElement(ElementName = "ptid")]
	public int PetTypeID;

	[XmlElement(ElementName = "o")]
	public int Order;
}
