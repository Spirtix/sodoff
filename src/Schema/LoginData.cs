using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "LoginData", IsNullable = true)]
[Serializable]
public class LoginData {

    [XmlElement(ElementName = "ChildName")]
    public string ChildName { get; set; }

    [XmlElement(ElementName = "Age")]
    public int? Age { get; set; }

    [XmlElement(ElementName = "UserName")]
    public string UserName;

    [XmlElement(ElementName = "Password")]
    public string Password;

    [XmlElement(ElementName = "Locale")]
    public string Locale;
}
