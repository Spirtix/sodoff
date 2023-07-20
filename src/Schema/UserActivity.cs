using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UA", Namespace = "")]
[Serializable]
public class UserActivity {
    [XmlElement(ElementName = "id", IsNullable = true)]
    public int? UserActivityID;

    [XmlElement(ElementName = "uid", IsNullable = true)]
    public Guid? UserID;

    [XmlElement(ElementName = "rid", IsNullable = true)]
    public Guid? RelatedUserID;

    [XmlElement(ElementName = "aid", IsNullable = true)]
    public int? UserActivityTypeID;

    [XmlElement(ElementName = "d", IsNullable = true)]
    public DateTime? LastActivityDate;
}
