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

    public virtual ICollection<Session> Sessions { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Dragon> Dragons { get; set; } = null!;
    public virtual ICollection<Image> Images { get; set; } = null!;
}
