using System.Diagnostics;
using System.Security.AccessControl;
using System.Xml.Serialization;

namespace sodoff.Schema;
[XmlRoot(ElementName = "ChallengeInfo", Namespace = "")]
[Serializable]
public class ChallengeInfo {
    [XmlElement(ElementName = "ChallengeID")]
    public int ChallengeID;

    [XmlElement(ElementName = "UserID")]
    public Guid UserID;

    [XmlElement(ElementName = "ProductGroupID")]
    public int ProductGroupID;

    [XmlElement(ElementName = "Points")]
    public int Points;

    [XmlElement(ElementName = "ExpirationDate")]
    public DateTime ExpirationDate;

    [XmlElement(ElementName = "ChallengeGameInfo")]
    public ChallengeGameInfo ChallengeGameInfo;

    [XmlElement(ElementName = "ChallengeContenders")]
    public ChallengeContenderInfo[] ChallengeContenders;

    [XmlElement(ElementName = "ExpirationDuration")]
    public int ExpirationDuration;
}
