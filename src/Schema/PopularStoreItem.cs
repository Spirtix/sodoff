using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class PopularStoreItem {
    [XmlElement(ElementName = "id")]
    public int ItemID;

    [XmlElement(ElementName = "c")]
    public int Rank;
}
