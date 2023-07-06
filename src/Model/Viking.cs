using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class Viking {
    [Key]
    public string Id { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = null!;

    public string? AvatarSerialized { get; set; }

    public int? SelectedDragonId { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Dragon> Dragons { get; set; } = null!;
    public virtual ICollection<Image> Images { get; set; } = null!;
    public virtual ICollection<MissionState> MissionStates { get; set; } = null!;
    public virtual Dragon? SelectedDragon { get; set; }

    public int InventoryId { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;
}
