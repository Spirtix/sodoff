using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ArrayOfMMOServerInfo", Namespace = "http://api.jumpstart.com/")]
[Serializable]
public class MMOServerInformation {

    [XmlElement(ElementName = "RZN")]
    public string RootZone { get; set; }

    [XmlElement(ElementName = "MMOServerInfo")]
    public MMOServerData[] MMOServerDataArray;
}
