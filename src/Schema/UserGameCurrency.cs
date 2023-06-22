using System.Xml.Serialization;

namespace sodoff.Schema;

[XmlRoot(ElementName = "UserGameCurrency", Namespace = "")]
[Serializable]
public class UserGameCurrency
{
	[XmlElement(ElementName = "id")]
	public int? UserGameCurrencyID;

	[XmlElement(ElementName = "uid")]
	public Guid? UserID;

	[XmlElement(ElementName = "gc")]
	public int? GameCurrency;

	[XmlElement(ElementName = "cc")]
	public int? CashCurrency;
}
