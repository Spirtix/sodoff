using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ImageData", Namespace = "", IsNullable = true)]
[Serializable]
public class ImageData {

	[XmlElement(ElementName = "ImageURL")]
	public string ImageURL;

	[XmlElement(ElementName = "TemplateName")]
	public string TemplateName;

	[XmlElement(ElementName = "SubType", IsNullable = true)]
	public string SubType;

	[XmlElement(ElementName = "PhotoFrame", IsNullable = true)]
	public string PhotoFrame;

	[XmlElement(ElementName = "PhotoFrameMask", IsNullable = true)]
	public string PhotoFrameMask;

	[XmlElement(ElementName = "Border", IsNullable = true)]
	public string Border;

	[XmlElement(ElementName = "Decal")]
	public ImageDataDecal[] Decal;
}
