using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UserLoginInfo", IsNullable = true)]
[Serializable]
public class UserLoginInfo {
    [XmlElement(ElementName = "UserName", IsNullable = true)]
    public string UserName;

    [XmlElement(ElementName = "FirstName", IsNullable = true)]
    public string FirstName;

    [XmlElement(ElementName = "LastName", IsNullable = true)]
    public string LastName;

    [XmlElement(ElementName = "Email", IsNullable = true)]
    public string Email;

    [XmlElement(ElementName = "ApiToken", IsNullable = true)]
    public string ApiToken;

    [XmlElement(ElementName = "UserID", IsNullable = true)]
    public string UserID;
}
