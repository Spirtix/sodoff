using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "DisplayNameUniqueResponse", Namespace = "")]
[Serializable]
public class DisplayNameUniqueResponse
{
	[XmlElement(ElementName = "suggestions", IsNullable = true)]
	public SuggestionResult Suggestions { get; set; }
}
