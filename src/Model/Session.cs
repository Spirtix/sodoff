using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class Session {
    [Key]
    public Guid ApiToken { get; set; }

    public Guid? UserId { get; set; }

    public int? VikingId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }

    public virtual Viking? Viking { get; set; }
}
