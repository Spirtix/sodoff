using System.Xml.Serialization;

namespace sodoff.Schema;

public enum PrerequisiteRequiredType {
    [XmlEnum("1")]
    Member = 1,
    [XmlEnum("2")]
    Accept,
    [XmlEnum("3")]
    Mission,
    [XmlEnum("4")]
    Rank,
    [XmlEnum("5")]
    DateRange,
    [XmlEnum("7")]
    Item = 7,
    [XmlEnum("8")]
    Event
}
