using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "MessageInfo", Namespace = "http://api.jumpstart.com/", IsNullable = true)]
[Serializable]
public class MessageInfo {
    [XmlElement(ElementName = "UserMessageQueueID", IsNullable = true)]
    public int? UserMessageQueueID;

    [XmlElement(ElementName = "FromUserID", IsNullable = true)]
    public string FromUserID;

    [XmlElement(ElementName = "MessageID", IsNullable = true)]
    public int? MessageID;

    [XmlElement(ElementName = "MessageTypeID", IsNullable = true)]
    public int? MessageTypeID;

    [XmlElement(ElementName = "MessageTypeName")]
    public string MessageTypeName;

    [XmlElement(ElementName = "MemberMessage")]
    public string MemberMessage;

    [XmlElement(ElementName = "NonMemberMessage")]
    public string NonMemberMessage;

    [XmlElement(ElementName = "MemberImageUrl")]
    public string MemberImageUrl;

    [XmlElement(ElementName = "NonMemberImageUrl")]
    public string NonMemberImageUrl;

    [XmlElement(ElementName = "MemberLinkUrl")]
    public string MemberLinkUrl;

    [XmlElement(ElementName = "NonMemberLinkUrl")]
    public string NonMemberLinkUrl;

    [XmlElement(ElementName = "MemberAudioUrl")]
    public string MemberAudioUrl;

    [XmlElement(ElementName = "NonMemberAudioUrl")]
    public string NonMemberAudioUrl;

    [XmlElement(ElementName = "Data")]
    public string Data;

}
