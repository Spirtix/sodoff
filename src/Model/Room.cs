using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class Room {
    [Key]
    public int Id { get; set; }

    public string RoomId { get; set; }

    public int VikingId { get; set; }

    public string? Name { get; set; }

    public virtual Viking? Viking { get; set; }

    public virtual ICollection<RoomItem> Items { get; set; } = null!;
}