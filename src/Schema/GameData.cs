using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GameData", Namespace = "")]
[Serializable]
public class GameData
{
	[XmlElement(ElementName = "RankID", IsNullable = true)]
	public int? RankID;

	[XmlElement(ElementName = "IsMember")]
	public bool IsMember;

	[XmlElement(ElementName = "UserName")]
	public string UserName;

	[XmlElement(ElementName = "Value")]
	public int Value;

	[XmlElement(ElementName = "DatePlayed", IsNullable = true)]
	public DateTime? DatePlayed;

	[XmlElement(ElementName = "Win")]
	public int Win;

	[XmlElement(ElementName = "Loss")]
	public int Loss;

	[XmlElement(ElementName = "UserID")]
	public Guid UserID;

	[XmlElement(ElementName = "ProductID", IsNullable = true)]
	public int? ProductID;

	[XmlElement(ElementName = "PlatformID", IsNullable = true)]
	public int? PlatformID;

	[XmlElement(ElementName = "FBIDS", IsNullable = true)]
	public long? FacebookID;
}
