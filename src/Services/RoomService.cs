using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace sodoff.Services;
public class RoomService {

    private readonly DBContext ctx;

    public RoomService(DBContext ctx) {
        this.ctx = ctx;
    }

    public void CreateRoom(Viking viking, string roomId) {
        viking.Rooms.Add(new Room { RoomId = roomId });
        ctx.SaveChanges();
    }

    public int[] CreateItems(UserItemPositionSetRequest[] roomItemRequest, Room room) {
        List<int> ids = new();
        foreach (var itemRequest in roomItemRequest) {
            // TODO: Remove item from inventory (using CommonInventoryID)
            InventoryItem? i = room.Viking?.Inventory.InventoryItems.FirstOrDefault(x => x.Id == itemRequest.UserInventoryCommonID);
            if (i != null) i.Quantity--; 
            UserItemPosition uip = itemRequest;
            RoomItem roomItem = new RoomItem {
                RoomItemData = XmlUtil.SerializeXml<UserItemPosition>(itemRequest).Replace(" xsi:type=\"UserItemPositionSetRequest\"", "") // NOTE: No way to avoid this hack when we're serializing a child class into a base class
            };

            room.Items.Add(roomItem);
            ctx.SaveChanges();
            ids.Add(roomItem.Id);
        }
        return ids.ToArray();
    }

    public UserItemState[] UpdateItems(UserItemPositionSetRequest[] roomItemRequest, Room room) {
        List<UserItemState> state = new();
        foreach (var itemRequest in roomItemRequest) {
            RoomItem? item = room.Items.FirstOrDefault(x => x.Id == itemRequest.UserItemPositionID);
            if (item is null) continue;

            UserItemPosition itemPosition = XmlUtil.DeserializeXml<UserItemPosition>(item.RoomItemData);

            if (itemRequest.Uses != null) itemPosition.Uses = itemRequest.Uses;
            itemPosition.InvLastModifiedDate = itemRequest.InvLastModifiedDate;
            if (itemRequest.UserItemState != null) itemPosition.UserItemState = itemRequest.UserItemState;
            if (itemRequest.UserItemAttributes != null) itemPosition.UserItemAttributes = itemRequest.UserItemAttributes;
            if (itemRequest.UserItemStat != null) itemPosition.UserItemStat = itemRequest.UserItemStat;
            if (itemRequest.Item != null) itemPosition.Item = itemRequest.Item;
            if (itemRequest.PositionX != null) itemPosition.PositionX = itemRequest.PositionX;
            if (itemRequest.PositionY != null) itemPosition.PositionY = itemRequest.PositionY;
            if (itemRequest.PositionZ != null) itemPosition.PositionZ = itemRequest.PositionZ;
            if (itemRequest.RotationX != null) itemPosition.RotationX = itemRequest.RotationX;
            if (itemRequest.RotationY != null) itemPosition.RotationY = itemRequest.RotationY;
            if (itemRequest.RotationZ != null) itemPosition.RotationZ = itemRequest.RotationZ;
            if (itemRequest.ParentID != null) itemPosition.ParentID = itemRequest.ParentID;

            item.RoomItemData = XmlUtil.SerializeXml(itemPosition);
            if (itemPosition.UserItemState != null) state.Add(itemPosition.UserItemState);
        }

        ctx.SaveChanges();
        return state.ToArray();
    }

    public void DeleteItems(int[] itemIds, Room room) {
        for (int i = 0; i < itemIds.Length; i++) {
            RoomItem? ri = room.Items.FirstOrDefault(x => x.Id == itemIds[i]);
            if (ri is null) continue;
            UserItemPosition itemPosition = XmlUtil.DeserializeXml<UserItemPosition>(ri.RoomItemData);
            room.Items.Remove(ri);
            InventoryItem? invItem = room.Viking?.Inventory.InventoryItems.FirstOrDefault(x => x.Id == itemPosition.UserInventoryCommonID);
            if (invItem != null) invItem.Quantity++;
        }
        ctx.SaveChanges();
    }

    public UserItemPositionList GetUserItemPositionList(Room room) {
        List<UserItemPosition> itemPosition = new();
        foreach (var item in room.Items) {
            UserItemPosition data = XmlUtil.DeserializeXml<UserItemPosition>(item.RoomItemData);
            data.UserItemPositionID = item.Id;
            itemPosition.Add(data);
        }
        return new UserItemPositionList { UserItemPosition = itemPosition.ToArray() };
    }
}
