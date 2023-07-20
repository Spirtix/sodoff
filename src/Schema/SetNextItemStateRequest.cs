using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class SetNextItemStateRequest {
    [XmlElement(ElementName = "SID")]
    public int StoreID { get; set; }

    [XmlElement(ElementName = "PGID")]
    public int ProductGroupID;

    [XmlElement(ElementName = "UID")]
    public Guid UserID;

    [XmlElement(ElementName = "CIID")]
    public int UserInventoryCommonID;

    [XmlElement(ElementName = "UIPID")]
    public int UserItemPositionID;

    [XmlElement(ElementName = "OSC")]
    public bool OverrideStateCriteria;

    [XmlElement(ElementName = "CICS")]
    public List<CommonInventoryConsumable> CommonInventoryConsumables;
}
