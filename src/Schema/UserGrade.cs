using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UGrad", Namespace = "", IsNullable = true)]
[Serializable]
public class UserGrade
{
	[XmlElement(ElementName = "UId", IsNullable = true)]
	public Guid? UserID;

	[XmlElement(ElementName = "UGID")]
	public int UserGradeID;

	[XmlElement(ElementName = "UGN")]
	public string UserGradeName;

	[XmlElement(ElementName = "UGL", IsNullable = true)]
	public bool? Locked;
}
