using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model;

public class Pair {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public int MasterId { get; set; }

    public virtual PairData PairData { get; set; }
}
