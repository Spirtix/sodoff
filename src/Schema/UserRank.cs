using System.Diagnostics;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UserRank", Namespace = "")]
[Serializable]
public class UserRank {
    [XmlElement(ElementName = "PointTypeID")]
    public AchievementPointTypes PointTypeID;

    [XmlElement(ElementName = "Value")]
    public int Value;

    [XmlElement(ElementName = "RankID")]
    public int? RankID;

    [XmlElement(ElementName = "GlobalRankID")]
    public int? GlobalRankID;
}
