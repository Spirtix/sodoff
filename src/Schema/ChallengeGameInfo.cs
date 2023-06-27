using System.Xml.Serialization;

namespace sodoff.Schema;
[XmlRoot(ElementName = "ChallengeGameInfo", IsNullable = false)]
[Serializable]
public class ChallengeGameInfo {
    [XmlElement(ElementName = "GameID")]
    public int GameID;

    [XmlElement(ElementName = "GameLevelID")]
    public int GameLevelID;

    [XmlElement(ElementName = "GameDifficultyID")]
    public int? GameDifficultyID;

    [XmlElement(ElementName = "GameName")]
    public string GameName;
}
