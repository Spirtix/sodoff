using System.Xml.Serialization;

namespace sodoff.Schema;


[XmlRoot(ElementName = "ChildRegistrationData", IsNullable = true)]
[Serializable]
public class ChildRegistrationData {
    [XmlElement(ElementName = "MultiplayerEnabled")]
    public bool? MultiplayerEnabled { get; set; }

    [XmlElement(ElementName = "isGenerateAvatar")]
    public bool? isGenerateAvatar { get; set; }

    [XmlElement(ElementName = "Locale")]
    public string Locale { get; set; }

    [XmlElement(ElementName = "Lock", IsNullable = true)]
    public bool? isLocked { get; set; }

    [XmlElement(ElementName = "Grade")]
    public int Grade { get; set; }

    [XmlElement(ElementName = "Age")]
    public int? Age;

    [XmlElement(ElementName = "BirthDate")]
    public DateTime BirthDate;

    [XmlElement(ElementName = "ChildName")]
    public string ChildName;

    [XmlElement(ElementName = "Gender")]
    public string Gender;

    [XmlElement(ElementName = "GuestUserName")]
    public string GuestUserName;

    [XmlElement(ElementName = "IsGuest")]
    public bool? IsGuest;

    [XmlElement(ElementName = "Password")]
    public string Password;

    [XmlElement(ElementName = "IsSuggestAvatarName")]
    public bool? IsSuggestAvatarName;
}

