using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "NameValidationRequest", Namespace = "")]
public class NameValidationRequest
{
	[XmlElement(ElementName = "Name")]
	public string Name;

	[XmlElement(ElementName = "Category")]
	public NameCategory Category;
}
