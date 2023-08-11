using System.Xml.Serialization;

namespace sodoff.Schema;

public enum AchievementPointTypes {
    [XmlEnum("1")]
    PlayerXP = 1,
    
    [XmlEnum("2")]
    GameCurrency = 2, // gold
    
    [XmlEnum("4")]
    Unused1 = 4, // unknown
    
    [XmlEnum("5")]
    CashCurrency = 5, // gems
    
    [XmlEnum("6")]
    ItemReward = 6,
    
    [XmlEnum("8")]
    DragonXP = 8,
    
    [XmlEnum("9")]
    PlayerFarmingXP = 9,
    
    [XmlEnum("10")]
    PlayerFishingXP = 10,
    
    [XmlEnum("12")]
    UDTPoints = 12,
}
