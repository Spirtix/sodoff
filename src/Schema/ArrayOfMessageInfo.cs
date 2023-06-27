using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(Namespace = "http://api.jumpstart.com/", IsNullable = true)]
[Serializable]
public class ArrayOfMessageInfo {
    [XmlElement(ElementName = "MessageInfo")]
    public MessageInfo[] MessageInfo;
}
