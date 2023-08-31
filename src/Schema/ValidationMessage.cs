using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "VMSG", Namespace = "")]
[Serializable]
public class ValidationMessage
{
	[XmlElement(ElementName = "ST", IsNullable = false)]
	public Status Status { get; set; }

	[XmlElement(ElementName = "EM", IsNullable = false)]
	public string Message { get; set; }
}
