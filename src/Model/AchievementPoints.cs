using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class AchievementPoints {
    public string VikingId { get; set; }

    public int Type { get; set; }

    public int Value { get; set; }

    public virtual Viking? Viking { get; set; }
}
