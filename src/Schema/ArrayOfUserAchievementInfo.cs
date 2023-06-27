using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfUserAchievementInfo", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class ArrayOfUserAchievementInfo {
    [XmlElement(ElementName = "UserAchievementInfo")]
    public UserAchievementInfo[] UserAchievementInfo;
}
