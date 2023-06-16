namespace sodoff.Schema;

using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;

[XmlRoot(Namespace = "http://api.jumpstart.com/", IsNullable = true)]
[Serializable]
public class UserInfo {
    [XmlElement(ElementName = "MembershipID", IsNullable = true)]
    public string MembershipID;
    [XmlElement(ElementName = "UserID", IsNullable = true)]
    public string UserID;
    [XmlElement(ElementName = "ParentUserID", IsNullable = true)]
    public string ParentUserID;
    [XmlElement(ElementName = "Username")]
    public string Username;
    [XmlElement(ElementName = "FirstName")]
    public string FirstName;
    [XmlElement(ElementName = "MultiplayerEnabled")]
    public bool MultiplayerEnabled;
    [XmlElement(ElementName = "BirthDate", IsNullable = true)]
    public DateTime? BirthDate;
    [XmlElement(ElementName = "GenderID", IsNullable = true)]
    public Gender? GenderID;
    [XmlElement(ElementName = "Age", IsNullable = true)]
    public int? Age;
    [XmlElement(ElementName = "Partner", IsNullable = true)]
    public string Partner;
    [XmlElement(ElementName = "lid", IsNullable = true)]
    public string Locale;
    [XmlElement(ElementName = "oce")]
    public bool OpenChatEnabled;
    [XmlElement(ElementName = "IsApproved")]
    public bool IsApproved;
    [XmlElement(ElementName = "RegistrationDate", IsNullable = true)]
    public DateTime? RegistrationDate;
    [XmlElement(ElementName = "FBID")]
    public long? FacebookUserID;
    [XmlElement(ElementName = "CreationDate", IsNullable = true)]
    public DateTime? CreationDate;
}

