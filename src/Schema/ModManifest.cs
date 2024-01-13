using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "sodoffmod", Namespace = "")]
public class ModManifest {
    [XmlArrayItem(ElementName = "item")]
    public ModItem [] items { get; set; }
}
