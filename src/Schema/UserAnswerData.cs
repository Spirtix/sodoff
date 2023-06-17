using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Answer", IsNullable = true, Namespace = "")]
[Serializable]
public class UserAnswerData
{
	[XmlElement(ElementName = "ID")]
	public string UserID;

	[XmlElement(ElementName = "Answers")]
	public ProfileUserAnswer[] Answers;
}
