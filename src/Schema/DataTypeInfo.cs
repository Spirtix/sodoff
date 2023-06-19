using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "DT")]
[Serializable]
public enum DataTypeInfo
{
	[XmlEnum("I")]
	Int = 1,
	
	[XmlEnum("2")]
	Float,
	
	[XmlEnum("3")]
	Double,
	
	[XmlEnum("4")]
	String
}
