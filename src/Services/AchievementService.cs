using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class AchievementService {
        private AchievementStoreSingleton achievementStore;
        private InventoryService inventoryService;

        Dictionary<AchievementPointTypes, UserRank[]> ranks = new();
        Dictionary<int, AchievementReward[]> achivmentsRewardByID = new();
        Dictionary<int, AchievementReward[]> achivmentsRewardByTask = new();

        int dragonAdultMinXP;
        int dragonTitanMinXP;

        public AchievementService(AchievementStoreSingleton achievementStore, InventoryService inventoryService) {
            this.achievementStore = achievementStore;
            this.inventoryService = inventoryService;
        }

        public UserAchievementInfo CreateUserAchievementInfo(string userId, int? value, AchievementPointTypes type) {
            if (value is null)
                value = 0;
            return new UserAchievementInfo {
                UserID = Guid.Parse(userId),
                AchievementPointTotal = value,
                RankID = achievementStore.GetRankFromXP(value, type),
                PointTypeID = type
            };
        }

        public UserAchievementInfo CreateUserAchievementInfo(Viking viking, AchievementPointTypes type) {
            return CreateUserAchievementInfo(viking.Id, viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type)?.Value, type);
        }

        public void DragonLevelUpOnAgeUp(Dragon dragon, RaisedPetGrowthState oldGrowthState, RaisedPetGrowthState newGrowthState) {
            if (oldGrowthState is null || newGrowthState.Order > oldGrowthState.Order) { // if dragon age up
                dragon.PetXP = achievementStore.GetUpdatedDragonXP(dragon.PetXP ?? 0, newGrowthState.Order);
            }
        }

        public void SetAchievementPoints(Viking viking, AchievementPointTypes type, int value) {
            if (type == AchievementPointTypes.DragonXP && viking.SelectedDragon != null) {
                viking.SelectedDragon.PetXP = value;
            } else if (type != null) {
                AchievementPoints xpPoints = viking.AchievementPoints.FirstOrDefault(a => a.Type == (int)type);
                if (xpPoints is null) {
                    xpPoints = new AchievementPoints {
                        Type = (int)type
                    };
                    viking.AchievementPoints.Add(xpPoints);
                }
                xpPoints.Value = value;
            }
        }

        public void AddAchievementPoints(Viking viking, AchievementPointTypes? type, int? value) {
            if (type == AchievementPointTypes.DragonXP && viking.SelectedDragon != null) {
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

        public AchievementReward AddAchievementPointsAndGetReward(Viking viking, AchievementPointTypes type, int value) {
            AddAchievementPoints(viking, type, value);
            return new AchievementReward{
                EntityID = Guid.Parse(viking.Id),
                PointTypeID = type,
                EntityTypeID = 1, // player ?
                RewardID = 1265, // TODO: placeholder
                Amount = value
            };
        }

        public void ApplyAchievementReward(Viking viking, AchievementReward reward) {
            if (reward.PointTypeID == AchievementPointTypes.ItemReward) {
                inventoryService.AddItemToInventory(viking, reward.ItemID, (int)reward.Amount!);
            } else { // currencies, all types of player XP and dragon XP
                AddAchievementPoints(viking, reward.PointTypeID, reward.Amount);
            }
        }

        public AchievementReward[] ApplyAchievementRewards(Viking viking, AchievementReward[] rewards) {
            if (rewards is null)
                return null;
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
            var rewards = achievementStore.GetAchievementRewardsById(achievementID);
            if (rewards != null) {
                return ApplyAchievementRewards(viking, rewards);
            } else {
                return new AchievementReward[0];
            }
        }

        public AchievementReward[] ApplyAchievementRewardsByTask(Viking viking, AchievementTask task) {
            var rewards = achievementStore.GetAchievementRewardsByTask(task.TaskID);
            if (rewards != null) {
                return ApplyAchievementRewards(viking, rewards);
            } else {
                return new AchievementReward[0];
            }
        }
        
        public UserGameCurrency GetUserCurrency(Viking viking) {
            // TODO: return real values (after implement currency collecting methods)
            return new UserGameCurrency {
                CashCurrency = 65536,
                GameCurrency = 65536,
                UserGameCurrencyID = 1, // TODO: user's wallet ID?
                UserID = Guid.Parse(viking.Id)
            };
        }
    }
}
