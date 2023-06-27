using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MSI", Namespace = "")]
[Serializable]
public class MMOServerData {
    [XmlElement(ElementName = "IP")]
    public string IPAddress { get; set; }

    [XmlElement(ElementName = "PN")]
    public int Port { get; set; }

    [XmlElement(ElementName = "VR")]
    public string Version { get; set; }

    [XmlElement(ElementName = "DF")]
    public bool isDefault { get; set; }

    [XmlElement(ElementName = "ZN")]
    public string ZoneName { get; set; }

    [XmlElement(ElementName = "RZN")]
    public string RootZone { get; set; }
}
