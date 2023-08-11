using System.Xml.Serialization;

namespace sodoff.Schema;

public enum AchievementPointTypes {
    PlayerXP = 1,
    GameCurrency = 2, // gold
    CashCurrency = 5, // gems
    ItemReward = 6,
    DragonXP = 8,
    PlayerFarmingXP = 9,
    PlayerFishingXP = 10,
}
