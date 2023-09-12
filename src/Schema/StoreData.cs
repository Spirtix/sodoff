using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "StoreData", Namespace = "")]
[Serializable]
public class StoreData {
    [XmlElement(ElementName = "i")]
    public int Id;

    [XmlElement(ElementName = "s")]
    public string StoreName;

    [XmlElement(ElementName = "d")]
    public string Description;

    [XmlElement(ElementName = "ii")]
    public int[] ItemId;

    [XmlElement(ElementName = "ss")]
    public ItemsInStoreDataSale[] SalesAtStore;

    [XmlElement(ElementName = "pitem")]
    public PopularStoreItem[] PopularItems;
}
