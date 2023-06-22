using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "SetTaskStateResult", Namespace = "")]
[Serializable]
public class SetTaskStateResult {
    [XmlElement(ElementName = "S")]
    public bool Success;

    [XmlElement(ElementName = "T")]
    public SetTaskStateStatus Status;

    [XmlElement(ElementName = "A")]
    public string AdditionalStatusParams;

    [XmlElement(ElementName = "R")]
    public MissionCompletedResult[] MissionsCompleted;

    [XmlElement(ElementName = "C")]
    public CommonInventoryResponse CommonInvRes;
}
