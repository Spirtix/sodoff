using System.Xml.Serialization;

namespace sodoff.Schema;

public enum AvatarValidationResult
{
	[XmlEnum("1")]
	Valid = 1,

	[XmlEnum("2")]
	PartInvalid,

	[XmlEnum("3")]
	PartTypeDuplicate,
	
	[XmlEnum("4")]
	PartTypeEmpty,
	
	[XmlEnum("5")]
	PartTypeLengthInvalid,
	
	[XmlEnum("6")]
	TexturesInvalid,
	
	[XmlEnum("7")]
	GeometriesInvalid,
	
	[XmlEnum("8")]
	OffsetsInvalid,
	
	[XmlEnum("9")]
	OffsetsNotFloat,
	
	[XmlEnum("10")]
	AvatarDisplayNameInvalid,
	
	[XmlEnum("255")]
	Error = 255
}
