using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "sdnr", Namespace = "")]
[Serializable]
public class SetDisplayNameRequest {
    [XmlElement(ElementName = "dn")]
    public string DisplayName { get; set; }

    [XmlElement(ElementName = "iid")]
    public int ItemID { get; set; }

    [XmlElement(ElementName = "sid")]
    public int StoreID { get; set; }
}
