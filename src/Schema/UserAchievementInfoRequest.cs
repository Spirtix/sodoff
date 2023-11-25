using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AREQ", Namespace = "")]
[Serializable]
public class UserAchievementInfoRequest {
    [XmlElement(ElementName = "PID")]
    public int ProductGroupID;

    [XmlElement(ElementName = "UID")]
    public Guid UserID;

    [XmlElement(ElementName = "PTID")]
    public int PointTypeID;

    [XmlElement(ElementName = "T")]
    public RequestType Type;

    [XmlElement(ElementName = "M")]
    public ModeType Mode;

    [XmlElement(ElementName = "P")]
    public int Page;

    [XmlElement(ElementName = "Q")]
    public int Quantity;

    [XmlElement(ElementName = "FBIDS")]
    public List<long> FacebookUserIDs;
}
