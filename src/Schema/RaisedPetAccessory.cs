using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RPAC", Namespace = "")]
[Serializable]
public class RaisedPetAccessory
{
	[XmlElement(ElementName = "tp")]
	public string Type;

	[XmlElement(ElementName = "g")]
	public string Geometry;

	[XmlElement(ElementName = "t")]
	public string Texture;

	[XmlElement(ElementName = "uiid", IsNullable = true)]
	public int? UserInventoryCommonID;

	[XmlElement(ElementName = "uid", IsNullable = true)]
	public UserItemData UserItemData;
}
