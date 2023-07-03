using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "pireq", Namespace = "")]
[Serializable]
public class PurchaseStoreItemRequest {
    [XmlElement(ElementName = "cid")]
    public int ContainerId { get; set; }

    [XmlElement(ElementName = "sid")]
    public int? StoreID { get; set; }

    [XmlElement(ElementName = "i")]
    public int[] Items { get; set; }

    [XmlElement(ElementName = "ct")]
    public int CurrencyType { get; set; }

    [XmlElement(ElementName = "ambi")]
    public bool AddMysteryBoxToInventory { get; set; }
}
