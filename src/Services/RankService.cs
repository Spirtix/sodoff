using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class RankService {

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();

        public RankService() {
            ArrayOfUserRank allranks = XmlUtil.DeserializeXml<ArrayOfUserRank>(XmlUtil.ReadResourceXmlString("allranks"));
            
            foreach (var pointType in Enum.GetValues<AchievementPointTypes>()) {
                ranks[pointType] = allranks.UserRank.Where(r => r.PointTypeID == pointType).ToArray();
            }
        }

        public int getRankFromXP(int? xpPoints, AchievementPointTypes type) {
            return ranks[type].Count(r => r.Value <= xpPoints);
        }

        public UserAchievementInfo userAchievementInfo(string userId, int? value, AchievementPointTypes type) {
            if (value is null)
                value = 0;
            return new UserAchievementInfo {
                UserID = Guid.Parse(userId),
                AchievementPointTotal = value,
                RankID = getRankFromXP(value, type),
                PointTypeID = type
            };
        }

        public UserAchievementInfo userAchievementInfo(Viking viking, AchievementPointTypes type) {
            return userAchievementInfo(viking.Id, viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type)?.Value, type);
        }

        static public void setAchievementPoints(Viking viking, AchievementPointTypes? type, int? value) {
            if (type == AchievementPointTypes.DragonXP) {
                viking.SelectedDragon.PetXP = viking.SelectedDragon.PetXP.GetValueOrDefault() + (int)value;
            } else {
                AchievementPoints xpPoints = viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type);
                if (xpPoints is null) {
                    xpPoints = new AchievementPoints {
                        Type = (int)type,
                        Value = 0
                    };
                    viking.AchievementPoints.Add(xpPoints);
                }
                xpPoints.Value += (int)value;
            }
        }
    }
}
