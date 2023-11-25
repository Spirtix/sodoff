using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "R", Namespace = "")]
[Serializable]
public class DateRange {
    [XmlElement(ElementName = "SD")]
    public DateTime? StartDate;

    [XmlElement(ElementName = "ED")]
    public DateTime? EndDate;
}
