using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "QL", IsNullable = true, Namespace = "")]
[Serializable]
public class ProfileQuestionList
{
	[XmlElement(ElementName = "ID", IsNullable = false)]
	public int ID;

	[XmlElement(ElementName = "Qs")]
	public ProfileQuestion[] Questions;

	public string Name;

	public string ImageURL;
}
