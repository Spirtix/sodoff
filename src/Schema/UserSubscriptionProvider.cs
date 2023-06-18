using System.Xml.Serialization;

namespace sodoff.Schema;

public class UserSubscriptionProvider
{
	[XmlElement(ElementName = "ItemID", IsNullable = false)]
	public string ItemID;

	[XmlElement(ElementName = "MembershipPurchaseAllowed", IsNullable = false)]
	public bool MembershipPurchaseAllowed;

	[XmlElement(ElementName = "ProductID", IsNullable = false)]
	public int ProductID;

	[XmlElement(ElementName = "Provider", IsNullable = false)]
	public int Provider;

	[XmlElement(ElementName = "ReceiptData", IsNullable = false)]
	public string ReceiptData;

	[XmlElement(ElementName = "Recurring", IsNullable = false)]
	public bool Recurring;

	[XmlElement(ElementName = "UserID", IsNullable = false)]
	public Guid UserID;
}
