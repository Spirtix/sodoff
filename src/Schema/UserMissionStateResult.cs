using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UserMissionStateResult", Namespace = "")]
[Serializable]
public class UserMissionStateResult {
    [XmlElement(ElementName = "UID")]
    public Guid UserID;

    [XmlElement(ElementName = "Mission")]
    public List<Mission> Missions;

    [XmlElement(ElementName = "UTA")]
    public List<UserTimedAchievement> UserTimedAchievement;

    // NOTE: It appears that the server doesn't send this
    // [XmlElement(ElementName = "D")]
    // public int Day;

    [XmlElement(ElementName = "MG")]
    public List<MissionGroup> MissionGroup;
}
