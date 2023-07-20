using System.Xml.Serialization;

namespace sodoff.Schema;

[Serializable]
public class SetNextItemStateResult {
    [XmlElement(ElementName = "EC")]
    public ItemStateChangeError ErrorCode { get; set; }

    [XmlElement(ElementName = "UIS")]
    public UserItemState UserItemState;

    [XmlElement(ElementName = "RS")]
    public AchievementReward[] Rewards;

    [XmlElement(ElementName = "S")]
    public bool Success;
}
