using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ItemStateCriteriaExpiry", Namespace = "")]
[Serializable]
public class ItemStateCriteriaExpiry : ItemStateCriteria
{
	[XmlElement(ElementName = "Period")]
	public int Period;

	[XmlElement(ElementName = "EndStateID")]
	public int EndStateID;
}
