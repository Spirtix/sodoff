using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class AchievementStoreSingleton {
        private InventoryService inventoryService;

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();
        Dictionary<int, AchievementReward[]> achivmentsRewardByID = new();
        Dictionary<int, AchievementReward[]> achivmentsRewardByTask = new();

        int dragonAdultMinXP;
        int dragonTitanMinXP;

        public AchievementStoreSingleton(InventoryService inventoryService) {
            this.inventoryService = inventoryService;

            ArrayOfUserRank allranks = XmlUtil.DeserializeXml<ArrayOfUserRank>(XmlUtil.ReadResourceXmlString("allranks"));
            foreach (var pointType in Enum.GetValues<AchievementPointTypes>()) {
                ranks[pointType] = allranks.UserRank.Where(r => r.PointTypeID == pointType).ToArray();
            }

            AchievementsIdInfo[] allAchievementsIdInfo = XmlUtil.DeserializeXml<AchievementsIdInfo[]>(XmlUtil.ReadResourceXmlString("achievementsids"));
            foreach (var achievementsIdInfo in allAchievementsIdInfo) {
                achivmentsRewardByID[achievementsIdInfo.AchievementID] = achievementsIdInfo.AchievementReward;
            }

            AchievementsTaskInfo[] allAchievementsTaskInfo = XmlUtil.DeserializeXml<AchievementsTaskInfo[]>(XmlUtil.ReadResourceXmlString("achievementstasks"));
            foreach (var achievementsTaskInfo in allAchievementsTaskInfo) {
                achivmentsRewardByTask[achievementsTaskInfo.TaskID] = achievementsTaskInfo.AchievementReward;
            }

            dragonAdultMinXP = ranks[AchievementPointTypes.DragonXP][10].Value;
            dragonTitanMinXP = ranks[AchievementPointTypes.DragonXP][20].Value;
        }

        public int GetRankFromXP(int? xpPoints, AchievementPointTypes type) {
            return ranks[type].Count(r => r.Value <= xpPoints);
        }

        public AchievementReward[]? GetAchievementRewardsById(int achievementID) {
            if (achivmentsRewardByID.ContainsKey(achievementID)) {
                return achivmentsRewardByID[achievementID];
            } else {
                return null;
            }
        }

        public AchievementReward[]? GetAchievementRewardsByTask(int taskID) {
            if (achivmentsRewardByTask.ContainsKey(taskID)) {
                return achivmentsRewardByTask[taskID];
            } else {
                return null;
            }
        }

        public int GetUpdatedDragonXP(int dragonXP, int growthState) {
            if (growthState == 4 && dragonXP < dragonAdultMinXP) {
                // to adult via ticket -> add XP
                dragonXP += dragonAdultMinXP;
            } else if  (growthState == 5 && dragonXP < dragonTitanMinXP) {
                // adult to titan via ticket -> add XP
                dragonXP += dragonTitanMinXP - dragonAdultMinXP;
            }
            return dragonXP;
        }
    }
}
