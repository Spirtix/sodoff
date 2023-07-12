using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class RoomItem {
    [Key]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public virtual Room Room { get; set; }

    public string RoomItemData { get; set; }
}
