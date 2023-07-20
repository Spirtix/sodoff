using System.Xml.Serialization;

namespace sodoff.Schema;

public enum ItemStateChangeError {
    [XmlEnum("1")]
    Success = 1,
    [XmlEnum("2")]
    CannotOverrideCriteria,
    [XmlEnum("3")]
    TimeNotReached,
    [XmlEnum("4")]
    CannotFindInventoryItem,
    [XmlEnum("5")]
    UsesLessThanRequired,
    [XmlEnum("6")]
    UnableToGetItem,
    [XmlEnum("7")]
    QtyLessThanRequired,
    [XmlEnum("8")]
    ItemNotInUserInventory,
    [XmlEnum("9")]
    TransitionFailed,
    [XmlEnum("10")]
    ItemStateNotInStateList,
    [XmlEnum("11")]
    OverrideCriteriaNotFound,
    [XmlEnum("12")]
    NoCriteriasFound,
    [XmlEnum("13")]
    AutomaticPurchaseFailed,
    [XmlEnum("14")]
    ItemStateExpired,
    [XmlEnum("255")]
    Error = 255
}

