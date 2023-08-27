using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class ItemService {

        Dictionary<int, ItemData> items = new();
        int[] itemsRewardForDT;
        Random random = new Random();

        public ItemService() {
            ServerItemArray itemArray = XmlUtil.DeserializeXml<ServerItemArray>(XmlUtil.ReadResourceXmlString("items"));
            foreach (var item in itemArray.ItemDataArray) {
                items.Add(item.ItemID, item);
            }

            itemsRewardForDT = XmlUtil.DeserializeXml<int[]>(XmlUtil.ReadResourceXmlString("dtrewards"));
        }

        public ItemData GetItem(int id) {
            return items[id];
        }

        public ItemData GetDTReward() {
            // TODO: better calculation of reward item - use difficulty of DT level, item rarity, tier, etc
            int itemID = itemsRewardForDT[random.Next(0, itemsRewardForDT.Length)];
            return items[itemID];
        }

        public ItemDataRelationship OpenBox(ItemData boxItem) {
            var boxRewards = boxItem.Relationship.Where(e => e.Type == "Prize").ToArray();
            int totalWeight = boxRewards.Sum(e => e.Weight);
            if (totalWeight == 0) {
                return boxRewards[random.Next(0, boxRewards.Length)];
            }
            int cnt = 0;
            int win = random.Next(0, totalWeight);
            foreach (var reward in boxRewards) {
                cnt += reward.Weight;
                if (cnt > win) {
                    return reward;
                }
            }
            return null;
        }

        public bool ItemHasCategory(ItemData itemData, int categoryId) {
            ItemDataCategory? category = itemData.Category?.FirstOrDefault(e => e.CategoryId == categoryId);
            return category != null;
        }

        public bool ItemHasCategory(ItemData itemData, int[] categoryIds) {
            ItemDataCategory? category = itemData.Category?.FirstOrDefault(e => categoryIds.Contains(e.CategoryId));
            return category != null;
        }

        public List<ItemStat> CreateItemStats(ItemPossibleStatsMap? possibleStats, int rarity, int itemTier) {
            List<ItemStat> newStat = new List<ItemStat>();
            int rMax = possibleStats.Stats.Sum(e => e.Probability);
            for (int i=0; i<rarity && rMax > 0; ++i) {
                int val = random.Next(0, rMax);
                int cnt = 0;
                foreach (var stat in possibleStats.Stats) {
                    if (newStat.FirstOrDefault(e => e.ItemStatID == stat.ItemStatsID) != null) {
                        // this type of stat already is in newStat ... is excluded from this draw
                        continue;
                    }

                    cnt += stat.Probability;
                    if (cnt > val) {
                        rMax -= stat.Probability; // do not include in the next draw

                        StatRangeMap rangeMap = stat.ItemStatsRangeMaps.FirstOrDefault(e => e.ItemTierID == itemTier);
                        newStat.Add(
                            new ItemStat {
                                ItemStatID = rangeMap.ItemStatsID,
                                Name = rangeMap.ItemStatsName,
                                Value = random.Next(rangeMap.StartRange, rangeMap.EndRange+1).ToString(),
                                DataType = DataTypeInfo.Int
                            }
                        );

                        break; // we found new stat for slot "i" ... goto i+1
                    }
                }
            }
            return newStat;
        }
    }
}
