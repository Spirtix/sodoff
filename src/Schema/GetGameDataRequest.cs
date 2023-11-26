using System.Xml.Serialization;

namespace sodoff.Schema;
[XmlRoot(ElementName = "GetGameDataRequest", Namespace = "")]
[Serializable]
public class GetGameDataRequest {
    [XmlElement(ElementName = "ProductGroupID")]
    public int? ProductGroupID { get; set; }

    [XmlElement(ElementName = "UserID")]
    public Guid? UserID { get; set; }

    [XmlElement(ElementName = "GameID")]
    public int? GameID { get; set; }

    [XmlElement(ElementName = "GameLevelID")]
    public int? GameLevelID { get; set; }

    [XmlElement(ElementName = "DifficultlyID")]
    public int? DifficultlyID { get; set; }

    [XmlElement(ElementName = "IsMultiplayer")]
    public bool? IsMultiplayer { get; set; }

    [XmlElement(ElementName = "TopScoresOnly")]
    public bool? TopScoresOnly { get; set; }

    [XmlElement(ElementName = "AllProductGroups")]
    public bool? AllProductGroups { get; set; }

    [XmlElement(ElementName = "AllUsers")]
    public bool? AllUsers { get; set; }

    [XmlElement(ElementName = "KEY")]
    public string Key { get; set; }

    [XmlElement(ElementName = "CNT")]
    public int? Count { get; set; }

    [XmlElement(ElementName = "SC")]
    public int? Score { get; set; }

    [XmlElement(ElementName = "AO")]
    public bool? AscendingOrder { get; set; }

    [XmlElement(ElementName = "FBIDS")]
    public List<long> FacebookUserIDs { get; set; }
}
