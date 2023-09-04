using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "rr", Namespace = "")]
[Serializable]
public class RedeemRequest {
	[XmlElement(ElementName = "i")]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "rc", IsNullable = true)]
	public int? RedeemItemFetchCount { get; set; }
}
