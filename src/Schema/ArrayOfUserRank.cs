using System.Diagnostics;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfUserRank", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfUserRank {
    [XmlElement(ElementName = "UserRank")]
    public UserRank[] UserRank;
}
