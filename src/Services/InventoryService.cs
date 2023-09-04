using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;
using System.Threading.Tasks;

namespace sodoff.Services {
    public class InventoryService {

        private readonly DBContext ctx;
        private ItemService itemService;

        public const int Shards = 13711;

        public InventoryService(DBContext ctx, ItemService itemService) {
            this.ctx = ctx;
            this.itemService = itemService;
        }

        public InventoryItem AddItemToInventory(Viking viking, int itemID, int quantity) {
            InventoryItem? item = null;
            if (!ItemNeedUniqueInventorySlot(itemID))
                item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.ItemId == itemID);
            if (item is null) {
                item = new InventoryItem {
                    ItemId = itemID,
                    Quantity = 0
                };
                viking.Inventory.InventoryItems.Add(item);
            }
            item.Quantity += quantity;
            return item;
        }

        public CommonInventoryResponseItem AddItemToInventoryAndGetResponse(Viking viking, int itemID, int quantity) {
            InventoryItem item = AddItemToInventory(viking, itemID, quantity);

            ctx.SaveChanges(); // We need to get the ID of the newly created item

            if (quantity == 1)
                quantity = 0; // The game expects 0 if quantity got updated by just 1
                              // Otherwise it expects the quantity from the request
            return new CommonInventoryResponseItem {
                CommonInventoryID = item.Id,
                ItemID = itemID,
                Quantity = quantity
            };
        }

        public InventoryItemStatsMap AddBattleItemToInventory(Viking viking, int itemId, int itemTier, ItemStat[] itemStat = null) {
            // get item data
            ItemData itemData = itemService.GetItem(itemId);

            // create new item
            InventoryItem item = new InventoryItem { ItemId = itemId, Quantity = 1 };
            ItemStatsMap itemStatsMap = new ItemStatsMap {
                ItemID = itemId,
                ItemTier = (ItemTier)itemTier,
                ItemStats = itemStat ?? itemService.CreateItemStats(itemData.PossibleStatsMap, (int)itemData.ItemRarity, itemTier).ToArray()
            };
            item.StatsSerialized = XmlUtil.SerializeXml(itemStatsMap);

            // add to viking
            viking.Inventory.InventoryItems.Add(item);
            ctx.SaveChanges(); // We need to get the ID of the newly created item

            // return item with stats
            return new InventoryItemStatsMap {
                CommonInventoryID = item.Id,
                Item = itemData,
                ItemStatsMap = itemStatsMap
            };
        }

        public void SellInventoryItem(Viking viking, int invItemID, ref int gold, ref int shard) {
            // get item from inventory
            InventoryItem? item = viking.Inventory.InventoryItems.FirstOrDefault(e => e.Id == invItemID);
            if (item is null)
                return;

            // get item data
            ItemData? itemData = itemService.GetItem(item.ItemId);

            // calculate shard price
            switch (itemData.ItemRarity) {
                case ItemRarity.Common:
                    shard += 1;
                    break;
                case ItemRarity.Rare:
                    shard += 3;
                    break;
                case ItemRarity.Epic:
                    shard += 5;
                    break;
                case ItemRarity.Legendary:
                    shard += 10;
                    break;
            }

            // TODO: calculate cash (gold) rewards

            // remove item
            viking.Inventory.InventoryItems.Remove(item);
        }

        public CommonInventoryData GetCommonInventoryData(Viking viking) {
            List<InventoryItem> items = viking.Inventory.InventoryItems.ToList();

            List<UserItemData> userItemData = new();
            foreach (InventoryItem item in items) {
                if (item.Quantity == 0) continue; // Don't include an item that the viking doesn't have
                ItemData itemData = itemService.GetItem(item.ItemId);
                UserItemData uid = new UserItemData {
                    UserInventoryID = item.Id,
                    ItemID = itemData.ItemID,
                    Quantity = item.Quantity,
                    Uses = itemData.Uses,
                    ModifiedDate = new DateTime(DateTime.Now.Ticks),
                    Item = itemData
                };
                if (item.StatsSerialized != null) {
                    ItemStatsMap itemStats = XmlUtil.DeserializeXml<ItemStatsMap>(item.StatsSerialized);
                    uid.ItemStats = itemStats.ItemStats;
                    uid.ItemTier = itemStats.ItemTier;
                } else if (itemData.ItemStatsMap != null) {
                    uid.ItemStats = itemData.ItemStatsMap?.ItemStats;
                    uid.ItemTier = itemData.ItemStatsMap?.ItemTier;
                }
                userItemData.Add(uid);
            }

            return new CommonInventoryData {
                UserID = Guid.Parse(viking.Id),
                Item = userItemData.ToArray()
            };
        }

        public bool ItemNeedUniqueInventorySlot(int itemId) {
            return itemService.ItemHasCategory(
                itemService.GetItem(itemId), new int[] {
                    541, // farm expansion
                    511, // dragons tactics (battle) items
                }
            );
        }
    }
}
