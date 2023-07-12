
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfUserItemPosition", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class UserItemPositionList {
    [XmlElement(ElementName = "UserItemPosition")]
    public UserItemPosition[] UserItemPosition;
}
