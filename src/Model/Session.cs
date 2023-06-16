using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class Session {
    [Key]
    public string ApiToken { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
