using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ParentLoginInfo", IsNullable = true)]
[Serializable]
public class ParentLoginInfo : UserLoginInfo {
    [XmlElement(ElementName = "LoginStatus")]
    public MembershipUserStatus Status;

    [XmlElement(ElementName = "ChildList")]
    public UserLoginInfo[] ChildList;

    [XmlElement(ElementName = "SendActivationReminder", IsNullable = true)]
    public bool? SendActivationReminder;

    [XmlElement(ElementName = "UnAuthorized", IsNullable = true)]
    public bool? UnAuthorized;
}
