using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Buddy", Namespace = "")]
[Serializable]
public class Buddy {
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "DisplayName")]
    public string DisplayName;

    [XmlElement(ElementName = "Status")]
    public BuddyStatus Status;

    [XmlElement(ElementName = "CreateDate")]
    public DateTime CreateDate;

    [XmlElement(ElementName = "Online")]
    public bool Online;

    [XmlElement(ElementName = "OnMobile")]
    public bool OnMobile;

    [XmlElement(ElementName = "BestBuddy")]
    public bool BestBuddy;
}
