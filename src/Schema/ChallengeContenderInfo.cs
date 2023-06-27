using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ChallengeContenderInfo", IsNullable = false)]
[Serializable]
public class ChallengeContenderInfo {
    [XmlElement(ElementName = "UserId")]
    public Guid UserID;

    [XmlElement(ElementName = "ChallengeID")]
    public int ChallengeID;

    [XmlElement(ElementName = "Points")]
    public int Points;

    [XmlElement(ElementName = "ChallengeState")]
    public ChallengeState ChallengeState;

    [XmlElement(ElementName = "ExpirationDate")]
    public DateTime ExpirationDate;
}
