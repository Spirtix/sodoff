using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class CommonInventoryConsumable {
    [XmlElement(ElementName = "CIID")]
    public int CommonInventoryID;

    [XmlElement(ElementName = "IID")]
    public int ItemID;
}
