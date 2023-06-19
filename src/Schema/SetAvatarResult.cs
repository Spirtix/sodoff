using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "SetAvatarResult", Namespace = "", IsNullable = false)]
[Serializable]
public class SetAvatarResult
{
	[XmlElement(ElementName = "Success")]
	public bool Success { get; set; }

	[XmlElement(ElementName = "StatusCode")]
	public AvatarValidationResult StatusCode { get; set; }

	[XmlElement(ElementName = "DisplayName", IsNullable = true)]
	public string DisplayName { get; set; }

	[XmlElement(ElementName = "Suggestions", IsNullable = true)]
	public SuggestionResult Suggestions { get; set; }
}
