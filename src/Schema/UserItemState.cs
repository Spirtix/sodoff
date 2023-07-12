using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class UserItemState {
    [XmlElement(ElementName = "CIID")]
    public int CommonInventoryID;

    [XmlElement(ElementName = "UIPID")]
    public int UserItemPositionID;

    [XmlElement(ElementName = "IID")]
    public int ItemID;

    [XmlElement(ElementName = "ISID")]
    public int ItemStateID;

    [XmlElement(ElementName = "SCD")]
    public DateTime StateChangeDate;
}
