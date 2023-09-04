﻿using sodoff.Schema;
using sodoff.Model;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class AchievementService {
        private AchievementStoreSingleton achievementStore;
        private InventoryService inventoryService;

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

        public AchievementReward? AddDragonAchievementPoints(Dragon dragon, int? value) {
            dragon.PetXP = (dragon.PetXP ?? 0) + (value ?? 0);

            return new AchievementReward{
                // NOTE: RewardID and EntityTypeID are not used by client
                EntityID = Guid.Parse(dragon.EntityId),
                PointTypeID = AchievementPointTypes.DragonXP,
                Amount = value ?? 0
            };
        }

        public AchievementReward? AddAchievementPoints(Viking viking, AchievementPointTypes? type, int? value) {
            if (type == AchievementPointTypes.DragonXP && viking.SelectedDragon != null) {
                return AddDragonAchievementPoints(viking.SelectedDragon, value);
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

                return new AchievementReward{
                    EntityID = Guid.Parse(viking.Id),
                    PointTypeID = type,
                    Amount = value
                };
            }
            return null;
        }

        public AchievementReward? ApplyAchievementReward(Viking viking, AchievementReward reward) {
            if (reward.PointTypeID == AchievementPointTypes.ItemReward) {
                inventoryService.AddItemToInventory(viking, reward.ItemID, (int)reward.Amount!);

                AchievementReward grantedReward = reward.Clone();
                grantedReward.EntityID = Guid.Parse(viking.Id);
                return grantedReward;
            } else { // currencies, all types of player XP and dragon XP
                return AddAchievementPoints(viking, reward.PointTypeID, reward.Amount);
            }
        }

        public AchievementReward[] ApplyAchievementRewards(Viking viking, AchievementReward[] rewards, Guid[]? dragonsIDs = null) {
            if (rewards is null)
                return null;

            List<AchievementReward> grantedRewards = new List<AchievementReward>();
            foreach (var reward in rewards) {
                if (dragonsIDs != null && reward.PointTypeID == AchievementPointTypes.DragonXP) {
                    double amountDouble = (reward.Amount ?? 0)/dragonsIDs.Length;
                    int amount = (int)Math.Ceiling(amountDouble);
                    foreach (Guid dragonID in dragonsIDs) {
                        Dragon dragon = viking.Dragons.FirstOrDefault(e => e.EntityId == dragonID.ToString());
                        grantedRewards.Add(
                            AddDragonAchievementPoints(dragon, amount)
                        );
                    }
                } else {
                    grantedRewards.Add(
                        ApplyAchievementReward(viking, reward)
                    );
                }
            }

            // TODO: check trophies, etc criteria and id need apply and add to results extra reward here

            return grantedRewards.ToArray();
        }

        public AchievementReward[] ApplyAchievementRewardsByID(Viking viking, int achievementID, Guid[]? dragonsIDs = null) {
            var rewards = achievementStore.GetAchievementRewardsById(achievementID);
            if (rewards != null) {
                return ApplyAchievementRewards(viking, rewards, dragonsIDs);
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
