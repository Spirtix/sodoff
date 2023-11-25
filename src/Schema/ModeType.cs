using System.Xml.Serialization;

namespace sodoff.Schema;

public enum ModeType {
    [XmlEnum("1")]
    AllTime = 1,
    [XmlEnum("2")]
    Daily,
    [XmlEnum("3")]
    Weekly,
    [XmlEnum("4")]
    Monthly
}
