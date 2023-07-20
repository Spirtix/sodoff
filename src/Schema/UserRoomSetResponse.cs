using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "URSR", Namespace = "", IsNullable = false)]
[Serializable]
public class UserRoomSetResponse {
    [XmlElement(ElementName = "S")]
    public bool Success { get; set; }

    [XmlElement(ElementName = "SC")]
    public UserRoomValidationResult StatusCode { get; set; }
}
