using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model;

public class Dragon {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid EntityId { get; set; }
    [Required]
    public int VikingId { get; set; }

    public string? RaisedPetData { get; set; }

    public int? PetXP { get; set; }
    
    public virtual Viking Viking { get; set; } = null!;
    public virtual ICollection<PairData> PairData { get; set; } = null!;
}
