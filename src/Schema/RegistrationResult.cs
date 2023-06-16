using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RegistrationResult", IsNullable = true)]
[Serializable]
public class RegistrationResult {

    [XmlElement(ElementName = "ParentLoginInfo")]
    public ParentLoginInfo ParentLoginInfo { get; set; }

    [XmlElement(ElementName = "UserID")]
    public string UserID { get; set; }

    [XmlElement(ElementName = "Status")]
    public MembershipUserStatus Status;

    // Token: 0x04002BDB RID: 11227
    [XmlElement(ElementName = "ApiToken")]
    public string ApiToken;
}
