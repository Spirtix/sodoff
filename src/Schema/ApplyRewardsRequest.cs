using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ARRES", IsNullable = true)]
[Serializable]
public class ApplyRewardsRequest {
	[XmlElement(ElementName = "GID", IsNullable = false)]
	public int GameID { get; set; }

	[XmlElement(ElementName = "LID", IsNullable = false)]
	public int LevelID { get; set; }

	[XmlElement(ElementName = "LDID", IsNullable = false)]
	public int LevelDifficultyID { get; set; }

	[XmlElement(ElementName = "LRT", IsNullable = false)]
	public LevelRewardType LevelRewardType { get; set; }

	[XmlElement(ElementName = "RPEMS", IsNullable = false)]
	public RaisedPetEntityMap[] RaisedPetEntityMaps { get; set; }

	[XmlElement(ElementName = "AGN", IsNullable = false)]
	public Gender AvatarGender { get; set; }

	[XmlElement(ElementName = "LOC", IsNullable = true)]
	public string Locale { get; set; }
}
