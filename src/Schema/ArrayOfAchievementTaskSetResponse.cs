using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfAchievementTaskSetResponse", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfAchievementTaskSetResponse
{
	[XmlElement(ElementName = "AchievementTaskSetResponse")]
	public AchievementTaskSetResponse[] AchievementTaskSetResponse;
}
