using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AvatarData", Namespace = "")]
[Serializable]
public class AvatarData
{
	[XmlElement(ElementName = "IsSuggestedAvatarName", IsNullable = true)]
    public bool? IsSuggestedAvatarName { get; set; }

    public int? Id;

	public string DisplayName;

	[XmlElement(ElementName = "Part")]
	public AvatarDataPart[] Part;

	[XmlElement(ElementName = "Gender")]
	public Gender GenderType;

	[XmlElement(ElementName = "UTD", IsNullable = true)]
	public bool? SetUserNameToDisplayName;
}
