using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GetGameDataResponse", Namespace = "")]
[Serializable]
public class GetGameDataResponse {
    [XmlElement(ElementName = "GameDataSummaryList")]
    public List<GameDataSummary> GameDataSummaryList { get; set; } = new List<GameDataSummary>();
}
