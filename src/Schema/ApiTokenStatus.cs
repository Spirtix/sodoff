using System.Xml.Serialization;

namespace sodoff.Schema;

public enum ApiTokenStatus {
    [XmlEnum("1")]
    TokenValid = 1,
    [XmlEnum("3")]
    TokenNotFound = 3
}
