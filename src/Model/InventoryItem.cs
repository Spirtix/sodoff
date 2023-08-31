using System.ComponentModel.DataAnnotations;

namespace sodoff.Model {
    public class InventoryItem {
        [Key]
        public int Id {  get; set; }

        public int ItemId { get; set; }

        public int InventoryId { get; set; }
        
        public string? StatsSerialized { get; set; }

        public virtual Inventory Inventory { get; set; }

        public int Quantity { get; set; }
    }
}
