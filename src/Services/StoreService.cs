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
            for (int i=0; i<s.ItemId.Length; ++i) {
                newStore.Items[i] = itemService.GetItem(s.ItemId[i]);
            }
            stores.Add(s.Id, newStore);
        }
    }

    public ItemsInStoreData GetStore(int id) {
        return stores[id];
    }
}
