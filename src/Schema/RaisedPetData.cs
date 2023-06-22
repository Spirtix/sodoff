using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "RPD", Namespace = "")]
[Serializable]
public class RaisedPetData
{
	[XmlElement(ElementName = "ispetcreated")]
	public bool IsPetCreated { get; set; }

	[XmlElement(ElementName = "validationmessage")]
	public string ValidationMessage { get; set; }

	[XmlElement(ElementName = "id")]
	public int RaisedPetID;

	[XmlElement(ElementName = "eid", IsNullable = true)]
	public Guid? EntityID;

	[XmlElement(ElementName = "uid")]
	public Guid UserID;

	[XmlElement(ElementName = "n")]
	public string Name;

	[XmlElement(ElementName = "ptid")]
	public int PetTypeID;

	[XmlElement(ElementName = "gs")]
	public RaisedPetGrowthState GrowthState;

	[XmlElement(ElementName = "ip", IsNullable = true)]
	public int? ImagePosition;

	[XmlElement(ElementName = "g")]
	public string Geometry;

	[XmlElement(ElementName = "t")]
	public string Texture;

	[XmlElement(ElementName = "gd")]
	public Gender Gender;

	[XmlElement(ElementName = "ac")]
	public RaisedPetAccessory[] Accessories;

	[XmlElement(ElementName = "at")]
	public RaisedPetAttribute[] Attributes;

	[XmlElement(ElementName = "c")]
	public RaisedPetColor[] Colors;

	[XmlElement(ElementName = "sk")]
	public RaisedPetSkill[] Skills;

	[XmlElement(ElementName = "st")]
	public RaisedPetState[] States;

	[XmlElement(ElementName = "is")]
	public bool IsSelected;

	[XmlElement(ElementName = "ir")]
	public bool IsReleased;

	[XmlElement(ElementName = "cdt")]
	public DateTime CreateDate;

	[XmlElement(ElementName = "updt")]
	public DateTime UpdateDate;
}
