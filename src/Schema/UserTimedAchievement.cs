using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UTA", Namespace = "")]
[Serializable]
public class UserTimedAchievement {
    [XmlElement(ElementName = "uid")]
    public Guid? UserID;

    [XmlElement(ElementName = "uta")]
    public int UserTimedAchievementMapID;

    [XmlElement(ElementName = "a")]
    public int AchievementID;

    [XmlElement(ElementName = "s")]
    public int Sequence;

    [XmlElement(ElementName = "st")]
    public int StatusID;

    [XmlElement(ElementName = "c")]
    public DateTime CreatedDate;

    [XmlElement(ElementName = "u", IsNullable = true)]
    public DateTime? UpdatedDate;

    [XmlElement(ElementName = "isdel")]
    public bool IsDeleted;

    [XmlElement(ElementName = "gid")]
    public int GroupID;

    [XmlElement(ElementName = "ar")]
    public List<AchievementReward> AchievementReward;

    [XmlElement(ElementName = "sid")]
    public int SetID;
}
