using System.Xml.Serialization;

namespace sodoff.Schema;

[Flags]
public enum Gender {
    [XmlEnum("0")]
    Unknown = 0,
    [XmlEnum("1")]
    Male = 1,
    [XmlEnum("2")]
    Female = 2
}
