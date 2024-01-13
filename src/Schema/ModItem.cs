using System.Xml.Serialization;

namespace sodoff.Schema;

public class ModItem {
    [XmlAttribute("action")]
    public ModAction action { get; set; } = ModAction.Default;
    
    [XmlElement(ElementName = "id")]
    public int? ItemID;
    
    [XmlElement(ElementName = "storeID")]
    public int[] stores;
    
    [XmlElement(ElementName = "data")]
    public ItemData data;
}
