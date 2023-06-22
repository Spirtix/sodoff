using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model;

public class Dragon {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string EntityId { get; set; } = null!;

    [Required]
    public string VikingId { get; set; } = null!;

    public string? SelectedVikingId { get; set; }

    public string? RaisedPetData { get; set; }

    public virtual Viking Viking { get; set; } = null!;
    public virtual Viking SelectedViking { get; set; } = null!;
}
