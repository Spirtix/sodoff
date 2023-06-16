using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ParentRegistrationData", IsNullable = true)]
[Serializable]
public class ParentRegistrationData {
    [XmlElement(ElementName = "Email")]
    public string Email { get; set; }

    [XmlElement(ElementName = "FirstName")]
    public string FirstName { get; set; }

    [XmlElement(ElementName = "LastName")]
    public string LastName { get; set; }

    [XmlElement(ElementName = "Password")]
    public string Password { get; set; }

    [XmlElement(ElementName = "Locale")]
    public string Locale { get; set; }

    [XmlElement(ElementName = "BirthDate")]
    public DateTime BirthDate { get; set; }

    [XmlElement(ElementName = "ReceivesEmail")]
    public bool ReceivesEmail { get; set; }

    [XmlElement(ElementName = "SubscriptionID")]
    public int SubscriptionID { get; set; }

    [XmlElement(ElementName = "ChildList")]
    public ChildRegistrationData[] ChildList { get; set; }

    [XmlElement(ElementName = "FromApiKey")]
    public Guid? FromApiKey { get; set; }

    [XmlElement(ElementName = "ToApiKey")]
    public Guid? ToApiKey { get; set; }

    [XmlElement(ElementName = "ApiToken")]
    public Guid? ApiToken { get; set; }

    [XmlElement(ElementName = "HasAcquisitionSource")]
    public bool? HasAcquisitionSource { get; set; }

    [XmlElement(ElementName = "FaceBookUserId")]
    public long? FaceBookUserId { get; set; }

    [XmlElement(ElementName = "FacebookAccessToken")]
    public string FacebookAccessToken { get; set; }

    [XmlElement(ElementName = "FavouriteTeamID", IsNullable = true)]
    public int? FavouriteTeamID { get; set; }

    [XmlElement(ElementName = "GroupID", IsNullable = true)]
    public Guid? GroupID { get; set; }

    [XmlElement(ElementName = "SendActivationEmail", IsNullable = true)]
    public bool? SendActivationEmail { get; set; }

    [XmlElement(ElementName = "AutoActivate", IsNullable = true)]
    public bool? AutoActivate { get; set; }

    [XmlElement(ElementName = "SendWelcomeEmail", IsNullable = true)]
    public bool? SendWelcomeEmail { get; set; }

    [XmlElement(ElementName = "IsAnonymous", IsNullable = true)]
    public bool? IsAnonymous { get; set; }

    [XmlElement(ElementName = "RequiredLogin", IsNullable = true)]
    public bool? RequiredLogin { get; set; }

    [XmlElement(ElementName = "LinkUserToFaceBook", IsNullable = true)]
    public bool? LinkUserToFaceBook { get; set; }

    [XmlElement(ElementName = "ClientIP", IsNullable = true)]
    public string ClientIP { get; set; }

    [XmlElement(ElementName = "ExternalUserID", IsNullable = true)]
    public string ExternalUserID { get; set; }

    [XmlElement(ElementName = "ExternalAuthData", IsNullable = true)]
    public string ExternalAuthData { get; set; }
}
