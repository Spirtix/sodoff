using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MGF", Namespace = "")]
[Serializable]
public class MissionGroup {
    [XmlElement(ElementName = "P")]
    public int ProductID;

    [XmlElement(ElementName = "MG")]
    public int MissionGroupID;

    [XmlElement(ElementName = "DMC")]
    public int DailyMissionCount;

    [XmlElement(ElementName = "CC")]
    public int CompletionCount;

    [XmlElement(ElementName = "D")]
    public int Day;

    [XmlElement(ElementName = "RC")]
    public int RewardCycle;
}
