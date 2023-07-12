using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UIPSR", Namespace = "")]
[Serializable]
public class UserItemPositionSetRequest : UserItemPosition {
    [XmlElement(ElementName = "pix")]
    public int? ParentIndex;
}
