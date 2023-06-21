using System.Xml.Serialization;

namespace sodoff.Schema;

[Flags]
public enum DataType
{
	[XmlEnum("1")]
	BOOL = 1,
	[XmlEnum("2")]
	BYTE = 2,
	[XmlEnum("3")]
	CHAR = 3,
	[XmlEnum("4")]
	DECIMAL = 4,
	[XmlEnum("5")]
	DOUBLE = 5,
	[XmlEnum("6")]
	FLOAT = 6,
	[XmlEnum("7")]
	INT = 7,
	[XmlEnum("8")]
	LONG = 8,
	[XmlEnum("9")]
	SBYTE = 9,
	[XmlEnum("10")]
	SHORT = 10,
	[XmlEnum("11")]
	STRING = 11,
	[XmlEnum("12")]
	UINT = 12,
	[XmlEnum("13")]
	ULONG = 13,
	[XmlEnum("14")]
	USHORT = 14
}
