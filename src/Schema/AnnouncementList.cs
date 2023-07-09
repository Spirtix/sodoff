using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "Announcements", Namespace = "")]
[Serializable]
public class AnnouncementList
{
	[XmlElement(ElementName = "Announcement")]
	public Announcement[] Announcements;
}