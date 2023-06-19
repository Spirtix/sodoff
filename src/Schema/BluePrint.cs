using System.Xml.Serialization;

namespace sodoff.Schema;


[XmlRoot(ElementName = "BP", Namespace = "", IsNullable = true)]
[Serializable]
public class BluePrint
{
	[XmlElement(ElementName = "BPDC", IsNullable = true)]
	public List<BluePrintDeductibleConfig> Deductibles { get; set; }

	[XmlElement(ElementName = "ING", IsNullable = false)]
	public List<BluePrintSpecification> Ingredients { get; set; }

	[XmlElement(ElementName = "OUT", IsNullable = false)]
	public List<BluePrintSpecification> Outputs { get; set; }
}
