using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfString", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ChildList {
    [XmlElement(ElementName = "string")]
    public string[] strings;
}
