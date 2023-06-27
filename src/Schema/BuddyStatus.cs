using System.Xml.Serialization;

namespace sodoff.Schema;

public enum BuddyStatus {
    [XmlEnum("0")]
    Unknown,
    [XmlEnum("1")]
    PendingApprovalFromOther,
    [XmlEnum("2")]
    PendingApprovalFromSelf,
    [XmlEnum("3")]
    Approved,
    [XmlEnum("4")]
    BlockedByOther,
    [XmlEnum("5")]
    BlockedBySelf,
    [XmlEnum("6")]
    BlockedByBoth
}
