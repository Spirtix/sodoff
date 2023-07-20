using System.Xml.Serialization;

namespace sodoff.Schema;
[XmlRoot(ElementName = "URR", Namespace = "")]
[Serializable]
public class UserRoomResponse {
    // Token: 0x040037EB RID: 14315
    [XmlElement(ElementName = "ur")]
    public List<UserRoom> UserRoomList;
}
