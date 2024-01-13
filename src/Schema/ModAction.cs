using System.Xml.Serialization;

namespace sodoff.Schema;

public enum ModAction {
    [XmlEnum("defualt")]
    Default = 0,
    
    [XmlEnum("add")]
    Add = 1,
    
    [XmlEnum("replace")]
    Replace = 2,
    
    [XmlEnum("remove")]
    Remove = 3,
}
