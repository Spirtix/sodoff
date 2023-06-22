using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "ImageDataDecalPosition", Namespace = "")]
[Serializable]
public class ImageDataDecalPosition
{
	[XmlElement(ElementName = "X")]
	public int X;

	[XmlElement(ElementName = "Y")]
	public int Y;
}
