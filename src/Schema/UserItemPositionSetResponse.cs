using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UIPSRS", Namespace = "")]
[Serializable]
public class UserItemPositionSetResponse {
    [XmlElement(ElementName = "s")]
    public bool Success;

    [XmlElement(ElementName = "ids")]
    public int[] CreatedUserItemPositionIDs;

    [XmlElement(ElementName = "r")]
    public ItemPositionValidationResult Result;

    [XmlElement(ElementName = "uciis")]
    public UserItemState[] UserItemStates;
}
