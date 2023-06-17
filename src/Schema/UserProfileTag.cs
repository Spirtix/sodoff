using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UPT", Namespace = "")]
[Serializable]
public class UserProfileTag
{
	[XmlElement(ElementName = "Date")]
	public DateTime CreateDate;

	[XmlElement(ElementName = "PGID")]
	public int ProductGroupID;

	[XmlElement(ElementName = "User")]
	public Guid UserID;

	[XmlElement(ElementName = "ID")]
	public int UserProfileTagID;

	[XmlElement(ElementName = "ProfileTag")]
	public List<ProfileTag> ProfileTags;
}
