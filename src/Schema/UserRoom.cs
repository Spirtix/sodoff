using System.Diagnostics;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UR", Namespace = "")]
[Serializable]
public class UserRoom {
    [XmlElement(ElementName = "N")]
    public string Name;

    [XmlElement(ElementName = "R")]
    public string RoomID;

    [XmlElement(ElementName = "CP")]
    public double CreativePoints;

    [XmlElement(ElementName = "C")]
    public int? CategoryID;

    [XmlElement(ElementName = "IID")]
    public int? ItemID;

    [XmlElement(ElementName = "SN")]
    public string SceneName;
}
