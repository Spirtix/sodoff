using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class ItemService {

        Dictionary<int, ItemData> items = new();
        Random random = new Random();

        public ItemService() {
            ServerItemArray itemArray = XmlUtil.DeserializeXml<ServerItemArray>(XmlUtil.ReadResourceXmlString("items"));
            foreach (var item in itemArray.ItemDataArray) {
                items.Add(item.ItemID, item);
            }
        }

        public ItemData GetItem(int id) {
            return items[id];
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
    }
}
