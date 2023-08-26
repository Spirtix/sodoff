using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GLRT")]
[Serializable]
public enum LevelRewardType {
	[XmlEnum("1")]
	LevelCompletion = 1,
	
	[XmlEnum("2")]
	LevelFailure = 2,
	
	[XmlEnum("3")]
	ExtraChest = 3
}
