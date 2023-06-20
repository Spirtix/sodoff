using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MissionCriteria", Namespace = "")]
[Serializable]
public class MissionCriteria {
    [XmlElement(ElementName = "Type")]
    public string Type;

    [XmlElement(ElementName = "Ordered")]
    public bool Ordered;

    [XmlElement(ElementName = "Min")]
    public int Min;

    [XmlElement(ElementName = "Repeat")]
    public int Repeat;

    [XmlElement(ElementName = "RuleItems")]
    public List<RuleItem> RuleItems;
}
