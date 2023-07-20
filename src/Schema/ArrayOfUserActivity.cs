using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfUserActivity", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfUserActivity {
    // Token: 0x040036D0 RID: 14032
    [XmlElement(ElementName = "UserActivity")]
    public UserActivity[] UserActivity;
}
