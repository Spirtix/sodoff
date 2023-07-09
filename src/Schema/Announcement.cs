using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Announcement")]
[Serializable]
public class Announcement
{
	[XmlElement(ElementName = "AnnouncementID")]
	public int AnnouncementID;

	[XmlElement(ElementName = "Description")]
	public string Description;

	[XmlElement(ElementName = "AnnouncementText")]
	public string AnnouncementText;

	[XmlElement(ElementName = "Type")]
	public AnnouncementType Type;

	[XmlElement(ElementName = "StartDate")]
	public DateTime StartDate;

	[XmlElement(ElementName = "EndDate", IsNullable = true)]
	public DateTime? EndDate;
}