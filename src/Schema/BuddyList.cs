using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(Namespace = "http://api.jumpstart.com/", ElementName = "ArrayOfBuddy", IsNullable = true)]
[Serializable]
public class BuddyList {
    [XmlElement(ElementName = "Buddy")]
    public Buddy[] Buddy;
}
