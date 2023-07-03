using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services;

public class StoreService {

    // NOTE: ANother memory waste (slow clap)
    Dictionary<int, ItemsInStoreData> stores = new();

    public StoreService() {
        GetStoreResponse storeArray = XmlUtil.DeserializeXml<GetStoreResponse>(XmlUtil.ReadResourceXmlString("store"));
        foreach (var s in storeArray.Stores)
            if (s.ID != null)
                stores.Add((int)s.ID, s);
    }

    public ItemsInStoreData GetStore(int id) {
        return stores[id];
    }
}
