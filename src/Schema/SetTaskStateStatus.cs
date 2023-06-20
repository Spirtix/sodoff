using System.Xml.Serialization;

namespace sodoff.Schema;

public enum SetTaskStateStatus {
    [XmlEnum("1")]
    RequiresMembership = 1,
    [XmlEnum("2")]
    RequiresAcceptance,
    [XmlEnum("3")]
    NotWithinDateRange,
    [XmlEnum("4")]
    PreRequisiteMissionIncomplete,
    [XmlEnum("5")]
    UserRankLessThanMinRank,
    [XmlEnum("6")]
    UserRankGreaterThanMaxRank,
    [XmlEnum("7")]
    UserHasNoRankData,
    [XmlEnum("8")]
    MissionStateNotFound,
    [XmlEnum("9")]
    RequiredPriorTaskIncomplete,
    [XmlEnum("10")]
    ParentsTaskIncomplete,
    [XmlEnum("11")]
    ParentsSubMissionIncomplete,
    [XmlEnum("12")]
    TaskCanBeDone,
    [XmlEnum("13")]
    OneOrMoreMissionsHaveNoRewardsAttached,
    [XmlEnum("14")]
    PayLoadUpdated,
    [XmlEnum("15")]
    NonRepeatableMission,
    [XmlEnum("255")]
    Unknown = 255
}
