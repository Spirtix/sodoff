using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RPR", Namespace = "")]
[Serializable]
public class RaisedPetRequest
{
	[XmlElement(ElementName = "ptid")]
	public int PetTypeID { get; set; }

	[XmlElement(ElementName = "uid")]
	public Guid? UserID { get; set; }

	[XmlElement(ElementName = "SASP")]
	public bool? SetAsSelectedPet { get; set; }

	[XmlElement(ElementName = "USOP")]
	public bool? UnSelectOtherPets { get; set; }

	[XmlElement(ElementName = "pgid")]
	public int? ProductGroupID { get; set; }

	[XmlElement(ElementName = "cid")]
	public int? ContainerId { get; set; }

	[XmlElement(ElementName = "cir")]
	public CommonInventoryRequest[] CommonInventoryRequests { get; set; }

	[XmlElement(ElementName = "rpd")]
	public RaisedPetData RaisedPetData { get; set; }
}
