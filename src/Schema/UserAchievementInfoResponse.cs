using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UAIR", Namespace = "")]
[Serializable]
public class UserAchievementInfoResponse {
    [XmlElement(ElementName = "UAI")]
    public UserAchievementInfo[] AchievementInfo;

    [XmlElement(ElementName = "DR")]
    public DateRange DateRange;
}
