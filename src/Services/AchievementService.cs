using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class AchievementService {

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();
        Dictionary<int, AchievementReward[]> achivmentsRewardByID = new();
        Dictionary<int, AchievementReward[]> achivmentsRewardByTask = new();

        int dragonAdultMinXP;
        int dragonTitanMinXP;

        public AchievementService() {
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
            if (oldGrowthState is null || newGrowthState.Order > oldGrowthState.Order) {
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

        public void ApplyAchievementReward(Viking viking, AchievementReward reward) {
            if (reward.PointTypeID == AchievementPointTypes.ItemReward) {
                // TODO: This is not a pretty solution. Use inventory service in the future
                InventoryItem? ii = viking.Inventory.InventoryItems.FirstOrDefault(x => x.ItemId == reward.ItemID);
                if (ii is null) {
                    ii = new InventoryItem {
                        ItemId = reward.ItemID,
                        Quantity = 0
                    };
                    viking.Inventory.InventoryItems.Add(ii);
                }
                ii.Quantity += (int)reward.Amount!;
            } else { // currencies, all types of player XP and dragon XP
                AddAchievementPoints(viking, reward.PointTypeID, reward.Amount);
            }
        }

        public AchievementReward[] ApplyAchievementRewards(Viking viking, AchievementReward[] rewards) {
            foreach (var reward in rewards) {
                ApplyAchievementReward(viking, reward);
                /* TODO we don't need this?
                if (reward.PointTypeID == AchievementPointTypes.DragonXP) {
                    reward.EntityID = Guid.Parse(viking.SelectedDragon.EntityId)
                } else {
                    reward.EntityID = Guid.Parse(viking.Id)
                } */
            }
            // TODO: check trophies, etc criteria and id need apply and add to results extra reward here
            return rewards;
        }

        public AchievementReward[] ApplyAchievementRewardsByID(Viking viking, int achievementID) {
            if (achivmentsRewardByID.ContainsKey(achievementID)) {
                var rewards = achivmentsRewardByID[achievementID];
                return ApplyAchievementRewards(viking, rewards);
            } else {
                Console.WriteLine(string.Format("Unknown rewards for achievementID={0}", achievementID));
                return new AchievementReward[0];
            }
        }

        public AchievementReward[] ApplyAchievementRewardsByTask(Viking viking, AchievementTask task) {
            if (achivmentsRewardByTask.ContainsKey(task.TaskID)) {
                var rewards = achivmentsRewardByTask[task.TaskID];
                return ApplyAchievementRewards(viking, rewards);
            } else {
                Console.WriteLine(string.Format("Unknown rewards for taskID={0}", task.TaskID));
                return new AchievementReward[0];
            }
        }
    }
}
