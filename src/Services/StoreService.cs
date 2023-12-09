using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services;

public class StoreService {
    Dictionary<int, ItemsInStoreData> stores = new();

    public StoreService(ItemService itemService) {
        StoreData[] storeArray = XmlUtil.DeserializeXml<StoreData[]>(XmlUtil.ReadResourceXmlString("store"));
        foreach (var s in storeArray) {
            var newStore = new ItemsInStoreData {
                ID = s.Id,
                StoreName = s.StoreName,
                Description = s.Description,
                Items = new ItemData[s.ItemId.Length],
                SalesAtStore = s.SalesAtStore,
                PopularItems = s.PopularItems
            };
            IEnumerable<ItemsInStoreDataSale>? memberSales = s.SalesAtStore?.Where(x => x.ForMembers == true);
            IEnumerable<ItemsInStoreDataSale>? normalSales = s.SalesAtStore?.Where(x => x.ForMembers == false || x.ForMembers == null);
            for (int i = 0; i < s.ItemId.Length; ++i) {
                newStore.Items[i] = itemService.GetItem(s.ItemId[i]);
                UpdateItemSaleModifier(newStore.Items[i], memberSales, normalSales);
            }
            stores.Add(s.Id, newStore);
        }
    }

    public ItemsInStoreData GetStore(int id) {
        return stores[id];
    }

    private bool IsSaleOutdated(ItemsInStoreDataSale sale) {
        if (sale.EndDate == null)
            return false;
        return sale.EndDate < DateTime.Now;
    }

    private void UpdateItemSaleModifier(ItemData item, IEnumerable<ItemsInStoreDataSale>? memberSales, IEnumerable<ItemsInStoreDataSale>? normalSales) {
        if (memberSales != null) {
            foreach (var memberSale in memberSales) {
                if (IsSaleOutdated(memberSale)) continue;
                if (item.Category != null && memberSale.CategoryIDs != null && item.Category.Any(x => memberSale.CategoryIDs.Contains(x.CategoryId))) {
                    item.MemberDiscountModifier = memberSale.Modifier;
                    break;
                }
                if (memberSale.ItemIDs != null && memberSale.ItemIDs.Contains(item.ItemID)) {
                    item.MemberDiscountModifier = memberSale.Modifier;
                    break;
                }
            }
        }
        if (normalSales != null) {
            foreach (var normalSale in normalSales) {
                if (IsSaleOutdated(normalSale)) continue;
                if (item.Category != null && normalSale.CategoryIDs != null && item.Category.Any(x => normalSale.CategoryIDs.Contains(x.CategoryId))) {
                    item.NormalDiscoutModifier = normalSale.Modifier;
                    break;
                }
                if (normalSale.ItemIDs != null && normalSale.ItemIDs.Contains(item.ItemID)) {
                    item.NormalDiscoutModifier = normalSale.Modifier;
                    break;
                }
            }
        }
    }
}
