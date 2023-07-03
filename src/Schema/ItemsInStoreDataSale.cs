using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "SL", Namespace = "", IsNullable = true)]
[Serializable]
public class ItemsInStoreDataSale {

    [XmlElement(ElementName = "pcid")]
    public int PriceChangeId;

    [XmlElement(ElementName = "m")]
    public float Modifier;

    [XmlElement(ElementName = "ic")]
    public string Icon;

    [XmlElement(ElementName = "rid", IsNullable = true)]
    public int? RankId;

    [XmlElement(ElementName = "iids")]
    public int[] ItemIDs;

    [XmlElement(ElementName = "cids")]
    public int[] CategoryIDs;

    [XmlElement(ElementName = "ism", IsNullable = true)]
    public bool? ForMembers;

    [XmlElement(ElementName = "sd", IsNullable = true)]
    public DateTime? StartDate;

    [XmlElement(ElementName = "ed", IsNullable = true)]
    public DateTime? EndDate;
}
