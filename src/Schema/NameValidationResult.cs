using System.Xml.Serialization;

namespace sodoff.Schema;

public enum NameValidationResult
{
	[XmlEnum("1")]
	Ok = 1,

	[XmlEnum("2")]
	Blocked,

	[XmlEnum("3")]
	NotUnique,

	[XmlEnum("4")]
	InvalidLength
}
