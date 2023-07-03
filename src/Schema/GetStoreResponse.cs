using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GetStoreResponse", Namespace = "", IsNullable = true)]
[Serializable]
public class GetStoreResponse {
    [XmlElement(ElementName = "Stores")]
    public ItemsInStoreData[] Stores;
}
