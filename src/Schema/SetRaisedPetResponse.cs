using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "SetRaisedPetResponse", Namespace = "")]
[Serializable]
public class SetRaisedPetResponse
{
	[XmlElement(ElementName = "ErrorMessage")]
	public string ErrorMessage { get; set; }

	[XmlElement(ElementName = "RaisedPetSetResult")]
	public RaisedPetSetResult RaisedPetSetResult { get; set; }

	[XmlElement(ElementName = "cir")]
	public CommonInventoryResponse UserCommonInventoryResponse { get; set; }
}
