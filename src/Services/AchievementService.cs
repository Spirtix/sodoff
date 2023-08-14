using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class AchievementService {

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();

        int dragonAdultMinXP;
        int dragonTitanMinXP;

        public AchievementService() {
            ArrayOfUserRank allranks = XmlUtil.DeserializeXml<ArrayOfUserRank>(XmlUtil.ReadResourceXmlString("allranks"));
            
            foreach (var pointType in Enum.GetValues<AchievementPointTypes>()) {
                ranks[pointType] = allranks.UserRank.Where(r => r.PointTypeID == pointType).ToArray();
            }

            dragonAdultMinXP = ranks[AchievementPointTypes.DragonXP][10].Value;
            dragonTitanMinXP = ranks[AchievementPointTypes.DragonXP][20].Value;
        }

        public int GetRankFromXP(int? xpPoints, AchievementPointTypes type) {
            return ranks[type].Count(r => r.Value <= xpPoints);
        }

        public UserAchievementInfo CreateUserAchievementInfo(string userId, int? value, AchievementPointTypes type) {
            if (value is null)
                value = 0;
            return new UserAchievementInfo {
                UserID = Guid.Parse(userId),
                AchievementPointTotal = value,
                RankID = GetRankFromXP(value, type),
                PointTypeID = type
            };
        }

        public UserAchievementInfo CreateUserAchievementInfo(Viking viking, AchievementPointTypes type) {
            return CreateUserAchievementInfo(viking.Id, viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type)?.Value, type);
        }

        public void DragonLevelUpOnAgeUp(Dragon dragon, RaisedPetGrowthState oldGrowthState, RaisedPetGrowthState newGrowthState) {
            if (newGrowthState.Order > oldGrowthState.Order) {
                // age up
                int dragonXP = dragon.PetXP ?? 0;
                if (newGrowthState.Order == 4 && dragonXP < dragonAdultMinXP) {
                    // to adult via ticket -> add XP
                    dragonXP += dragonAdultMinXP;
                } else if  (newGrowthState.Order == 5 && dragonXP < dragonTitanMinXP) {
                    // adult to titan via ticket -> add XP
                    dragonXP += dragonTitanMinXP - dragonAdultMinXP;
                }
                dragon.PetXP = dragonXP;
            }
        }

        public void AddAchievementPoints(Viking viking, AchievementPointTypes? type, int? value) {
            if (type == AchievementPointTypes.DragonXP) {
                viking.SelectedDragon.PetXP = (viking.SelectedDragon.PetXP ?? 0) + (value ?? 0);
            } else if (type != null) {
                AchievementPoints xpPoints = viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type);
                if (xpPoints is null) {
                    xpPoints = new AchievementPoints {
                        Type = (int)type,
                        Value = 0
                    };
                    viking.AchievementPoints.Add(xpPoints);
                }
                xpPoints.Value += value ?? 0;
            }
        }
    }
}
