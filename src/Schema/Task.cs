using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Task", Namespace = "")]
[Serializable]
public class Task {
    [XmlElement(ElementName = "I")]
    public int TaskID;

    [XmlElement(ElementName = "N")]
    public string Name;

    [XmlElement(ElementName = "S")]
    public string Static;

    [XmlElement(ElementName = "C")]
    public int Completed;

    [XmlElement(ElementName = "F")]
    public bool Failed;

    [XmlElement(ElementName = "P")]
    public string Payload;
}
