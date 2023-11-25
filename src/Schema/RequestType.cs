using System.Xml.Serialization;

namespace sodoff.Schema;

public enum RequestType {
    [XmlEnum("1")]
    All = 1,
    [XmlEnum("2")]
    Buddy,
    [XmlEnum("3")]
    Group,
    [XmlEnum("4")]
    Facebook,
    [XmlEnum("5")]
    HallOfFame
}
