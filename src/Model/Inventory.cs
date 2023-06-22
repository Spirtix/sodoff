using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class Inventory {
    [Key]
    public int Id { get; set; }

    public string VikingId { get; set; }

    public virtual Viking? Viking { get; set; }

    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = null!;
}
