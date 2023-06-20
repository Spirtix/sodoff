using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "PrerequisiteItem", Namespace = "")]
[Serializable]
public class PrerequisiteItem {
    [XmlElement(ElementName = "Type")]
    public PrerequisiteRequiredType Type;

    [XmlElement(ElementName = "Value")]
    public string Value;

    [XmlElement(ElementName = "Quantity")]
    public short Quantity;

    [XmlElement(ElementName = "ClientRule")]
    public bool ClientRule;
}
