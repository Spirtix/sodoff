using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model;

[Index(nameof(UserId))]
[Index(nameof(VikingId))]
public class PairData {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int PairId { get; set; }

    public Guid? UserId { get; set; }

    public int? VikingId { get; set; }

    public int? DragonId { get; set; }

    public virtual ICollection<Pair> Pairs { get; set; }

    public virtual User? User { get; set; }

    public virtual Viking? Viking { get; set; }

    public virtual Dragon? Dragon { get; set; }
}
