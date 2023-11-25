using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;
public class GameData {
    [Key]
    public int Id { get; set; }

    public int VikingId { get; set; }

    public int GameId { get; set; }
    public int Difficulty {  get; set; }
    public int GameLevel {  get; set; }
    public DateTime DatePlayed {  get; set; }
    public bool IsMultiplayer { get; set; }
    public bool Win {  get; set; }
    public bool Loss { get; set; }
    public virtual ICollection<GameDataPair> GameDataPairs { get; set; } = null!;
    public virtual Viking Viking { get; set; } = null!;

}
