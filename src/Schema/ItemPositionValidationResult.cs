using System.Xml.Serialization;

namespace sodoff.Schema;

public enum ItemPositionValidationResult {
    [XmlEnum("1")]
    Valid = 1,
    [XmlEnum("2")]
    PositionIDInNewItem,
    [XmlEnum("3")]
    PositionIDInvalid,
    [XmlEnum("4")]
    ParentIndexOutofRange,
    [XmlEnum("5")]
    ParentIndexInvalid,
    [XmlEnum("6")]
    CommonInventoryIDInvalid,
    [XmlEnum("7")]
    ParentIDInvalid
}
