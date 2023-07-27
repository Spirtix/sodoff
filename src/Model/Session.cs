using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class Session {
    [Key]
    public string ApiToken { get; set; } = null!;

    public string? UserId { get; set; }

    public string? VikingId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }

    public virtual Viking? Viking { get; set; }
}
