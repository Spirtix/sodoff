using System.Xml.Serialization;

namespace sodoff.Schema;

[Flags]
public enum AnnouncementType
{
	[XmlEnum("0")]
	Unknown = 0,
	[XmlEnum("1")]
	Text = 1,
	[XmlEnum("2")]
	VoiceOver = 2,
	[XmlEnum("3")]
	GeneralText = 3,
	[XmlEnum("4")]
	ScavengerText = 4,
	[XmlEnum("5")]
	Video = 5,
	[XmlEnum("6")]
	EggNapping = 6
}