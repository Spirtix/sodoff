using System.Xml.Serialization;

namespace sodoff.Schema;

public class ParentLoginData : LoginData {
    [XmlElement(ElementName = "ChildUserID")]
    public Guid? ChildUserID { get; set; }

    [XmlElement(ElementName = "FaceBookUserId")]
    public long? FaceBookUserId { get; set; }

    [XmlElement(ElementName = "FacebookAccessToken")]
    public string FacebookAccessToken { get; set; }

    [XmlElement(ElementName = "LoginDuration")]
    public int? LoginDuration { get; set; }

    [XmlElement(ElementName = "LoginHash")]
    public string LoginHash { get; set; }

    [XmlElement(ElementName = "ClientIP")]
    public string ClientIP { get; set; }

    [XmlElement(ElementName = "ExternalUserID", IsNullable = true)]
    public string ExternalUserID { get; set; }

    [XmlElement(ElementName = "ExternalAuthData", IsNullable = true)]
    public string ExternalAuthData { get; set; }
}
