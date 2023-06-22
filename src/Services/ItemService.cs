using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services {
    public class ItemService {

        Dictionary<int, ItemData> items = new();

        public ItemService() {
            ServerItemArray itemArray = XmlUtil.DeserializeXml<ServerItemArray>(XmlUtil.ReadResourceXmlString("items"));
            foreach (var item in itemArray.ItemDataArray) {
                items.Add(item.ItemID, item);
            }
        }

        public ItemData GetItem(int id) {
            return items[id];
        }
    }
}
