using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "GetStoreRequest", Namespace = "")]
[Serializable]
public class GetStoreRequest {
    [XmlElement(ElementName = "StoreIDs")]
    public int[] StoreIDs;

    [XmlElement(ElementName = "PIF")]
    public bool GetPopularItems = true;
}
