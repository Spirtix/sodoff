using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GameDataSummary", Namespace = "")]
[Serializable]
public class GameDataSummary
{
	[XmlElement(ElementName = "GameDataList")]
	public GameData[] GameDataList;

	[XmlElement(ElementName = "UserPosition", IsNullable = true)]
	public int? UserPosition;

	[XmlElement(ElementName = "GameID")]
	public int GameID;

	[XmlElement(ElementName = "IsMultiplayer")]
	public bool IsMultiplayer;

	[XmlElement(ElementName = "Difficulty")]
	public int Difficulty;

	[XmlElement(ElementName = "GameLevel")]
	public int GameLevel;

	[XmlElement(ElementName = "Key")]
	public string Key;
}
