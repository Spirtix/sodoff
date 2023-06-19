using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "CI", Namespace = "")]
[Serializable]
public class CommonInventoryData
{
	[XmlElement(ElementName = "uid")]
	public Guid UserID;

	[XmlElement(ElementName = "i")]
	public UserItemData[] Item;
}
