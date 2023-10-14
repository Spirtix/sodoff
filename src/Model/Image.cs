using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace sodoff.Model;

[PrimaryKey(nameof(ImageType), nameof(ImageSlot), nameof(VikingId))]
public class Image {
    [Required]
    public string ImageType { get; set; } = null!;

    [Required]
    public int ImageSlot { get; set; }

    [Required]
    public int VikingId { get; set; }

    public string? ImageData { get; set; }

    public string? TemplateName { get; set; }

    public virtual Viking Viking { get; set; } = null!;
}
