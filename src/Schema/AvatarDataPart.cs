using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AvatarDataPart", Namespace = "")]
[Serializable]
public class AvatarDataPart
{
	public string PartType;

	[XmlArrayItem("Offset")]
	public AvatarDataPartOffset[] Offsets;

	[XmlArrayItem("Geometry")]
	public string[] Geometries;

	[XmlArrayItem("Texture")]
	public string[] Textures;

	[XmlArrayItem("Attribute")]
	public AvatarPartAttribute[] Attributes;

	[XmlElement(ElementName = "Uiid", IsNullable = true)]
	public int? UserInventoryId;

	public const string SAVED_DEFAULT_PREFIX = "DEFAULT_";

	public const string PLACEHOLDER = "PLACEHOLDER";
}
