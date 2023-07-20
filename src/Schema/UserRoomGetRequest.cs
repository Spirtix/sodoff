using System.Xml.Serialization;

namespace sodoff.Schema;
[XmlRoot(ElementName = "URGR", Namespace = "")]
[Serializable]
public class UserRoomGetRequest {
    [XmlElement(ElementName = "UID")]
    public Guid? UserID { get; set; }

    [XmlElement(ElementName = "CID")]
    public int? CategoryID { get; set; }
}
