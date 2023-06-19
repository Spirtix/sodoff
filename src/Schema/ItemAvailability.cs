using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Availability", Namespace = "")]
[Serializable]
public class ItemAvailability
{
	[XmlElement(ElementName = "sdate", IsNullable = true)]
	public DateTime? StartDate;

	[XmlElement(ElementName = "edate", IsNullable = true)]
	public DateTime? EndDate;
}
