using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Answers", IsNullable = true, Namespace = "")]
[Serializable]
public class ProfileAnswer
{
	[XmlElement(ElementName = "ID")]
	public int ID;

	[XmlElement(ElementName = "T")]
	public string DisplayText;

	[XmlElement(ElementName = "Img")]
	public string ImageURL;

	[XmlElement(ElementName = "L")]
	public string Locale;

	[XmlElement(ElementName = "O")]
	public int Ordinal;

	[XmlElement(ElementName = "QID")]
	public int QuestionID;
}
