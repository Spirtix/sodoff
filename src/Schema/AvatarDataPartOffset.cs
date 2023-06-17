using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "AvatarDataPartOffset", Namespace = "")]
[Serializable]
public class AvatarDataPartOffset
{
	public float X;

	public float Y;

	public float Z;
}
