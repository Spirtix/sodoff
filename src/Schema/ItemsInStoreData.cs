using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "S", Namespace = "", IsNullable = true)]
[Serializable]
public class ItemsInStoreData {
    [XmlElement(ElementName = "i", IsNullable = true)]
    public int? ID;

    [XmlElement(ElementName = "s")]
    public string StoreName;

    [XmlElement(ElementName = "d")]
    public string Description;

    [XmlElement(ElementName = "is")]
    public ItemData[] Items;

    [XmlElement(ElementName = "ss")]
    public ItemsInStoreDataSale[] SalesAtStore;

    [XmlElement(ElementName = "pitem")]
    public PopularStoreItem[] PopularItems;
}
