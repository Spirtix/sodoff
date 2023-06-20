using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MissionCompletedResult", Namespace = "")]
[Serializable]
public class MissionCompletedResult {
    [XmlElement(ElementName = "M")]
    public int MissionID;

    [XmlElement(ElementName = "A")]
    public AchievementReward[] Rewards;
}
